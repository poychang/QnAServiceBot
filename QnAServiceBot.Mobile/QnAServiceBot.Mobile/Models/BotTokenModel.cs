using Newtonsoft.Json;

namespace QnAServiceBot.Mobile.Models
{
    public class BotTokenModel
    {
        public string ConversationId { get; set; }

        public string Token { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn { get; set; }
    }
}
