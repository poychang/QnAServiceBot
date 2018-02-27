using System;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Connector;

namespace QnAMakerBot.AgentModule.Models
{
    /// <summary>服務人員</summary>
    [Serializable]
    public class Agent
    {
        public Agent()
        {
        }

        public Agent(IActivity activity)
        {
            ConversationReference = activity.ToConversationReference();
            AgentId = activity.From.Id;
        }

        /// <summary>服務人員識別碼</summary>
        public string AgentId { get; set; }

        /// <summary>對話框的參考</summary>
        public ConversationReference ConversationReference { get; set; }
    }
}
