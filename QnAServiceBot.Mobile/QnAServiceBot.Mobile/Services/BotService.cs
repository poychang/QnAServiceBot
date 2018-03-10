using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QnAServiceBot.Mobile.Models;

namespace QnAServiceBot.Mobile.Services
{
    public class BotService
    {
        private HttpClient _httpClient;
        private Conversation _lastConversation;

        public async Task Setup(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException();
            }

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Constants.BOT_DIRECTLINE);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var conversation = new Conversation();
            HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(conversation), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/conversations/", contentPost);
            if (response.IsSuccessStatusCode)
            {
                var conversationInfo = await response.Content.ReadAsStringAsync();
                _lastConversation = JsonConvert.DeserializeObject<Conversation>(conversationInfo);
            }
        }

        public async Task<BotTokenModel> FetchBotToken(UserModel user, CancellationToken cancellationToken)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(
                new Uri($@"{Constants.API_URL}/BotToken/generate/{user.Username}"),
                new StringContent(JsonConvert.SerializeObject("")), cancellationToken);

            if (!response.IsSuccessStatusCode) return null;

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BotTokenModel>(data);
        }

        public async Task<Activity> SendMessage(UserModel user, string messageText)
        {
            var messageToSend = new Activity()
            {
                Type = "message",
                From = user,
                Text = messageText
            };
            return await SendConversation(messageToSend);
        }

        public async Task<Activity> SendConnect(UserModel user, bool isConnected)
        {
            var messageToSend = new Activity()
            {
                Type = "event",
                From = user,
                Name = isConnected ? "disconnect" : "connect"
            };
            return await SendConversation(messageToSend);
        }

        public async Task<Activity> SendStopConversation(UserModel user)
        {
            var messageToSend = new Activity()
            {
                Type = "event",
                From = user,
                Name = "stopConversation"
            };
            return await SendConversation(messageToSend);
        }

        private async Task<Activity> SendConversation(Activity activity)
        {
            var contentPost = new StringContent(JsonConvert.SerializeObject(activity), Encoding.UTF8, "application/json");
            var conversationUrl = Constants.BOT_DIRECTLINE + "/" + _lastConversation.ConversationId + "/activities";

            var response = await _httpClient.PostAsync(conversationUrl, contentPost);
            var activityId = await response.Content.ReadAsStringAsync();

            var received = await _httpClient.GetAsync(conversationUrl);
            var receivedData = await received.Content.ReadAsStringAsync();
            var activitySet = JsonConvert.DeserializeObject<ActivitySet>(receivedData);
            var activities = activitySet.Activities;

            return activities.FirstOrDefault(m => m.From.Id == Constants.BOT_ID);
        }

        // TODO: 使用 WebSocket 維持與 Bot 的連線，持續接收來自 Bot 的訊息
    }
}
