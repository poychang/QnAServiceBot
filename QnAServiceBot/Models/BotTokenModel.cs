using Newtonsoft.Json;

namespace QnAServiceBot.Models
{
    public class BotTokenModel
    {
        public string ConversationId { get; set; }

        public string Token { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
    }
}
