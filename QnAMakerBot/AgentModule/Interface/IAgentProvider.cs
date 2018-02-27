using QnAMakerBot.AgentModule.Models;

namespace QnAMakerBot.AgentModule.Interface
{
    /// <summary>管理下一個有空的服務人員資料</summary>
    public interface IAgentProvider
    {
        /// <summary>取得下一位有空的服務人員</summary>
        /// <returns>服務人員</returns>
        Agent GetNextAvailableAgent();

        /// <summary>增加一位指定服務人員至可用人員池</summary>
        /// <param name="agent">服務人員</param>
        /// <returns>是否成功</returns>
        bool AddAgent(Agent agent);

        /// <summary>從可用人員池移除一位指定的服務人員</summary>
        /// <param name="agent">服務人員</param>
        /// <returns>服務人員</returns>
        Agent RemoveAgent(Agent agent);
    }
}
