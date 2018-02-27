using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Threading;
using System.Threading.Tasks;
using QnAMakerBot.AgentModule.Models;

namespace QnAMakerBot.AgentModule
{
    /// <summary>工具</summary>
    public static class Utility
    {
        /// <summary>傳送訊息至指定的對話階段</summary>
        /// <param name="activity">活動訊息</param>
        /// <returns></returns>
        public static async Task SendToConversationAsync(Activity activity)
        {
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            await connector.Conversations.SendToConversationAsync(activity);
        }

        /// <summary>取得機器人詳細資訊</summary>
        /// <param name="userAddress">某個對話階段的識別碼</param>
        /// <param name="botDataStore">機器人詳細資訊的儲存庫</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public static async Task<IBotData> GetBotDataAsync(IAddress userAddress, IBotDataStore<BotData> botDataStore, CancellationToken cancellationToken)
        {
            var botData = new JObjectBotData(userAddress, botDataStore);
            await botData.LoadAsync(cancellationToken);
            return botData;
        }

        /// <summary>取得使用者在某個對話階段的識別碼</summary>
        /// <param name="user">使用者</param>
        /// <returns></returns>
        public static IAddress GetAddress(this User user) =>
            Address.FromActivity(user.ConversationReference.GetPostToBotMessage());

        /// <summary>取得服務人員在某個對話階段的識別碼</summary>
        /// <param name="agent">服務人員</param>
        /// <returns></returns>
        public static IAddress GetAddress(this Agent agent) =>
            Address.FromActivity(agent.ConversationReference.GetPostToBotMessage());
    }
}
