using System;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Connector;

namespace QnAMakerBot.AgentModule.Models
{
    /// <summary>使用者</summary>
    [Serializable]
    public class User
    {
        public User()
        {
        }

        public User(IActivity message)
        {
            ConversationReference = message.ToConversationReference();
        }

        /// <summary>對話框的參考</summary>
        public ConversationReference ConversationReference { get; set; }
    }
}
