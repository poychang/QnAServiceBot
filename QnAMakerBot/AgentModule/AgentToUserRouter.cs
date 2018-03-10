using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using QnAMakerBot.AgentModule.Interface;

namespace QnAMakerBot.AgentModule
{
    /// <inheritdoc />
    public class AgentToUserRouter : IAgentToUser
    {
        private readonly IAgentService _agentService;

        public AgentToUserRouter(IAgentService agentService)
        {
            _agentService = agentService;
        }

        /// <inheritdoc />
        public async Task SendToUserAsync(Activity message, CancellationToken cancellationToken)
        {
            var user = await _agentService.GetUserInConversationAsync(message, cancellationToken);
            var reference = user.ConversationReference;
            var reply = reference.GetPostToUserMessage();
            reply.Text = message.Text;

            await Utility.SendToConversationAsync(reply);
        }
    }
}
