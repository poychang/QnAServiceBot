using System.Threading;
using System.Threading.Tasks;
using QnAMakerBot.AgentModule.Models;

namespace QnAMakerBot.AgentModule.Interface
{
    /// <summary>管理 Agent User Mapping 的對照資料</summary>
    public interface IAgentUserMapping
    {
        /// <summary>從 Agent User Mapping 中取得對照到的服務人員</summary>
        /// <param name="user">使用者</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<Agent> GetAgentFromMappingAsync(User user, CancellationToken cancellationToken);

        /// <summary>從 Agent User Mapping 中取得對照到的使用者</summary>
        /// <param name="agent">服務人員</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<User> GetUserFromMappingAsync(Agent agent, CancellationToken cancellationToken);

        /// <summary>設定 Agent User Mapping 中使用者與服務人員的對照</summary>
        /// <param name="agent">服務人員</param>
        /// <param name="user">使用者</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task SetAgentUserMappingAsync(Agent agent, User user, CancellationToken cancellationToken);

        /// <summary>移除 Agent User Mapping 中使用者與服務人員的對照</summary>
        /// <param name="agent">服務人員</param>
        /// <param name="user">使用者</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task RemoveAgentUserMappingAsync(Agent agent, User user, CancellationToken cancellationToken);

        /// <summary>判斷服務人員在 Agent User Mapping 中是否有對照資料</summary>
        /// <param name="agent">服務人員</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<bool> DoesMappingExist(Agent agent, CancellationToken cancellationToken);

        /// <summary>判斷使用者在 Agent User Mapping 中是否有對照資料</summary>
        /// <param name="user">使用者</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<bool> DoesMappingExist(User user, CancellationToken cancellationToken);
    }
}
