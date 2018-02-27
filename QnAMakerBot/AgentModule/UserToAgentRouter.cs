using Microsoft.Bot.Connector;
using QnAMakerBot.AgentModule.Interface;
using System.Threading;
using System.Threading.Tasks;
using QnAMakerBot.AgentModule.Models;

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
    }
}
