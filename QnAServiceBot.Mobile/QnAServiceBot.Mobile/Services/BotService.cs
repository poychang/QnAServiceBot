using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using QnAServiceBot.Mobile.Models;

namespace QnAServiceBot.Mobile.Services
{
    public class BotService
    {
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
    }
}
