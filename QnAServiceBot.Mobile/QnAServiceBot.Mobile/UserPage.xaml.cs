using Newtonsoft.Json;
using QnAServiceBot.Mobile.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QnAServiceBot.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public UserPage(UserModel user)
        {
            InitializeComponent();

            CurrentUser = user;
        }

        private UserModel CurrentUser { get; }
        private BotTokenModel BotToken { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitPage();
        }

        private async void InitPage()
        {
            await FetchBotToken();

            MyStackLayout.Children.Add(new WebView()
            {
                Source = $"https://webchat.botframework.com/embed/KingstonFE-QnA?t={BotToken.Token}",
                VerticalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = 300,
                HeightRequest = 1000
            });
        }

        private async Task FetchBotToken()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(
                new Uri($@"{Constants.API_URL}/BotToken/generate/{CurrentUser.Username}"),
                new StringContent(JsonConvert.SerializeObject("")));

            if (!response.IsSuccessStatusCode) return;

            var data = await response.Content.ReadAsStringAsync();
            BotToken = JsonConvert.DeserializeObject<BotTokenModel>(data);
        }
    }
}
