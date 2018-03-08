using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QnAServiceBot.Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QnAServiceBot.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public UserPage ()
        {
            InitializeComponent ();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            FetchBotToken();
        }

        private async void FetchBotToken()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(
                new Uri($@"{Constants.API_URL}/BotToken/generate/poy"),
                new StringContent(JsonConvert.SerializeObject("")));

            if (!response.IsSuccessStatusCode) return;

            var data = await response.Content.ReadAsStringAsync();
            var botToken = JsonConvert.DeserializeObject<BotTokenModel>(data);
            EntryToken.Text = data;
        }
    }
}