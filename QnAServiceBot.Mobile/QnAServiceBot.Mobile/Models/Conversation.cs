using Newtonsoft.Json;

namespace QnAServiceBot.Mobile.Models
{
    public class Conversation
    {
        public string ConversationId { get; set; }
        public string Token { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn { get; set; }

        public string StreamUrl { get; set; }
    }
}
