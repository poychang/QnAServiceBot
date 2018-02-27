using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace QnAMakerBot.AgentModule.Interface
{
    /// <summary>服務人員傳送活動訊息給使用者的相關方法</summary>
    public interface IAgentToUser
    {
        /// <summary>傳送活動訊息給使用者</summary>
        /// <param name="message">活動訊息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task SendToUserAsync(Activity message, CancellationToken cancellationToken);
    }
}
