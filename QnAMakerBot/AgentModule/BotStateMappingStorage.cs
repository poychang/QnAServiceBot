using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using QnAMakerBot.AgentModule.Interface;
using QnAMakerBot.AgentModule.Models;

namespace QnAMakerBot.AgentModule
{
    /// <inheritdoc />
    public class BotStateMappingStorage : IAgentUserMapping
    {
        private readonly IBotDataStore<BotData> _botDataStore;

        public BotStateMappingStorage(IBotDataStore<BotData> botDataStore)
        {
            _botDataStore = botDataStore;
        }

        /// <inheritdoc />
        public async Task<bool> DoesMappingExist(User user, CancellationToken cancellationToken)
        {
            var userAddress = user.GetAddress();
            var botData = await Utility.GetBotDataAsync(userAddress, _botDataStore, cancellationToken);
            return botData.PrivateConversationData.ContainsKey(Constants.AGENT_KEY);
        }

        /// <inheritdoc />
        public async Task<bool> DoesMappingExist(Agent agent, CancellationToken cancellationToken)
        {
            var agentAddress = agent.GetAddress();
            var botData = await Utility.GetBotDataAsync(agentAddress, _botDataStore, cancellationToken);
            return botData.PrivateConversationData.ContainsKey(Constants.USER_KEY);
        }

        /// <inheritdoc />
        public async Task<Agent> GetAgentFromMappingAsync(User user, CancellationToken cancellationToken)
        {
            var userAddress = user.GetAddress();
            var botData = await Utility.GetBotDataAsync(userAddress, _botDataStore, cancellationToken);
            botData.PrivateConversationData.TryGetValue(Constants.AGENT_KEY, out Agent agent);
            return agent;
        }

        /// <inheritdoc />
        public async Task<User> GetUserFromMappingAsync(Agent agent, CancellationToken cancellationToken)
        {
            var agentAddress = agent.GetAddress();
            var botData = await Utility.GetBotDataAsync(agentAddress, _botDataStore, cancellationToken);
            botData.PrivateConversationData.TryGetValue(Constants.USER_KEY, out User user);
            return user;
        }

        /// <inheritdoc />
        public async Task RemoveAgentUserMappingAsync(Agent agent, User user, CancellationToken cancellationToken)
        {
            var userAddress = user.GetAddress();
            var agentAddress = agent.GetAddress();

            var userData = await Utility.GetBotDataAsync(userAddress, _botDataStore, cancellationToken);
            var agentData = await Utility.GetBotDataAsync(agentAddress, _botDataStore, cancellationToken);

            userData.PrivateConversationData.RemoveValue(Constants.AGENT_KEY);
            agentData.PrivateConversationData.RemoveValue(Constants.USER_KEY);

            await userData.FlushAsync(cancellationToken);
            await agentData.FlushAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task SetAgentUserMappingAsync(Agent agent, User user, CancellationToken cancellationToken)
        {
            var agentAddress = agent.GetAddress();
            var userAddress = user.GetAddress();

            await SetAgentToUserStateAsync(userAddress, agent, cancellationToken);
            await SetUserToAgentStateAsync(agentAddress, user, cancellationToken);
        }
        
        private async Task SetAgentToUserStateAsync(IAddress userAddress, Agent agent, CancellationToken cancellationToken)
        {
            var botData = await Utility.GetBotDataAsync(userAddress, _botDataStore, cancellationToken);
            botData.PrivateConversationData.SetValue(Constants.AGENT_KEY, agent);
            await botData.FlushAsync(cancellationToken);
        }

        private async Task SetUserToAgentStateAsync(IAddress agentAddress, User user, CancellationToken cancellationToken)
        {
            var botData = await Utility.GetBotDataAsync(agentAddress, _botDataStore, cancellationToken);
            botData.PrivateConversationData.SetValue(Constants.USER_KEY, user);
            await botData.FlushAsync(cancellationToken);
        }
    }
}
