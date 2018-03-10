using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;
using QnAMakerBot.AgentModule.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace QnAMakerBot.AgentModule.Scorable
{
    /// <summary>全域處理程序 - 客服人員→使用者</summary>
    /// <remarks>https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-scorable-dialogs</remarks>
    public class AgentToUserScorable : ScorableBase<IActivity, bool, double>
    {
        private readonly IAgentService _agentService;
        private readonly IAgentToUser _agentToUser;
        private readonly IBotToUser _botToUser;

        public AgentToUserScorable(IAgentToUser agentToUser, IAgentService agentService, IBotToUser botToUser)
        {
            _agentToUser = agentToUser;
            _agentService = agentService;
            _botToUser = botToUser;
        }

        /// <summary>準備階段：分析對話是否進入此處理程序</summary>
        protected override async Task<bool> PrepareAsync(IActivity item, CancellationToken token) =>
            await _agentService.IsAgent(item, token);

        /// <summary>取得此對話處理程序是否有分數</summary>
        protected override bool HasScore(IActivity item, bool state) => state;

        /// <summary>取得此對話處理程序的分數</summary>
        protected override double GetScore(IActivity item, bool state) => state ? 1.0 : 0.0;

        /// <summary>執行階段：主要的對話處理程序，會從所有全域處理程序中執行最高分的項目</summary>
        protected override async Task PostAsync(IActivity item, bool state, CancellationToken token)
        {
            if (await _agentService.IsInExistingConversationAsync(item, token))
                await _agentToUser.SendToUserAsync(item as Activity, token);
            else
            {
                await _botToUser.PostAsync(ConversationText.NotTalkingWithAnyUser);
                await _botToUser.PostAsync(ConversationText.NotTalkingWithBot);
            }
        }

        /// <summary>完成階段：可在此階段清除所占用的資源</summary>
        protected override Task DoneAsync(IActivity item, bool state, CancellationToken token) => Task.CompletedTask;
    }
}
