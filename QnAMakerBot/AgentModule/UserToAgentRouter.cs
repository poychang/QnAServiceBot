using System.Net.Http;
using Microsoft.Bot.Connector;
using QnAMakerBot.AgentModule.Interface;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Microsoft.Bot.Builder.Luis.Models;
using QnAMakerBot.AgentModule.Models;
using Microsoft.Bot.Builder.Azure;

namespace QnAMakerBot.AgentModule
{
    /// <inheritdoc/>
    public class UserToAgentRouter : IUserToAgent
    {
        private readonly IAgentProvider _agentProvider;
        private readonly IAgentService _agentService;
        private readonly IAgentUserMapping _agentUserMapping;

        public UserToAgentRouter(IAgentProvider agentProvider, IAgentService agentService, IAgentUserMapping agentUserMapping)
        {
            _agentProvider = agentProvider;
            _agentService = agentService;
            _agentUserMapping = agentUserMapping;
        }

        /// <inheritdoc/>
        public async Task<bool> AgentTransferRequiredAsync(Activity message, CancellationToken cancellationToken)
        {
            return await _agentUserMapping.DoesMappingExist(new User(message), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Agent> IntitiateConversationWithAgentAsync(Activity message, CancellationToken cancellationToken)
        {
            var agent = _agentProvider.GetNextAvailableAgent();
            if (agent == null)
                return null;

            await _agentUserMapping.SetAgentUserMappingAsync(agent, new User(message), cancellationToken);

            var userReply = message.CreateReply(string.Format(ConversationText.YouConnectToSomeone, agent.ConversationReference.User.Name));
            await Utility.SendToConversationAsync(userReply);

            var agentReply = agent.ConversationReference.GetPostToUserMessage();
            agentReply.Text = string.Format(ConversationText.SomeoneJoinTheConversation, message.From.Name);
            await Utility.SendToConversationAsync(agentReply);

            return agent;
        }

        /// <inheritdoc/>
        public async Task SendToAgentAsync(Activity message, CancellationToken cancellationToken)
        {
            var agent = await _agentService.GetAgentInConversationAsync(message, cancellationToken);
            var reference = agent.ConversationReference;
            var reply = reference.GetPostToUserMessage();
            reply.Text = message.Text;

            await Utility.SendToConversationAsync(reply);
        }

        /// <inheritdoc/>
        public async Task<bool> IsWantToTalkWithHuman(Activity message, CancellationToken cancellationToken)
        {
            using (var httpClient = new HttpClient())
            {
                var luisAppId = Utils.GetAppSetting("LuisAppId");
                var luisSubscriptionKey = Utils.GetAppSetting("LuisSubscriptionKey");
                const string luisEndpoint = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/";

                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", luisSubscriptionKey);
                var response = await httpClient.GetAsync($@"{luisEndpoint}{luisAppId}/?verbose=true&q={message.Text}", cancellationToken);

                if (!response.IsSuccessStatusCode) return false;

                // get LUIS response content as string
                var contents = await response.Content.ReadAsStringAsync();
                var result = new JavaScriptSerializer().Deserialize<LuisResult>(contents);
                return result.TopScoringIntent.Intent.Equals("Want To Talk With Human") && result.TopScoringIntent.Score > 0.5;
            }
        }
    }
}
