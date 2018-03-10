using Microsoft.Bot.Connector;
using System.Threading;
using System.Threading.Tasks;
using QnAMakerBot.AgentModule.Models;

namespace QnAMakerBot.AgentModule.Interface
{
    /// <summary>使用者傳送活動訊息給服務人員的相關方法</summary>
    public interface IUserToAgent
    {
        /// <summary>檢查是否有配對一位服務人員</summary>
        /// <param name="message">活動訊息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<bool> AgentTransferRequiredAsync(Activity message, CancellationToken cancellationToken);

        /// <summary>傳送活動訊息給服務人員</summary>
        /// <param name="message">活動訊息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task SendToAgentAsync(Activity message, CancellationToken cancellationToken);

        /// <summary>初始化與服務人員連線的對話階段</summary>
        /// <param name="message">活動訊息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<Agent> IntitiateConversationWithAgentAsync(Activity message, CancellationToken cancellationToken);

        /// <summary>判斷是否想要轉接真人</summary>
        /// <param name="message">活動訊息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<bool> IsWantToTalkWithHuman(Activity message, CancellationToken cancellationToken);
    }
}
