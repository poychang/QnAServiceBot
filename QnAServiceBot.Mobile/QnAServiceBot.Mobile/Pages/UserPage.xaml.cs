using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QnAServiceBot.Mobile.Models;
using QnAServiceBot.Mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QnAServiceBot.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public UserPage(UserModel user)
        {
            InitializeComponent();

            CurrentUser = user;
            BotService = new BotService();
        }

        private UserModel CurrentUser { get; }
        private BotTokenModel BotToken { get; set; }
        private BotService BotService { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitPage();
        }

        private async void InitPage()
        {
            BotToken = await BotService.FetchBotToken(CurrentUser, CancellationToken.None);

            MyStackLayout.Children.Add(new WebView()
            {
                Source = $"https://webchat.botframework.com/embed/KingstonFE-QnA?t={BotToken.Token}",
                VerticalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = 300,
                HeightRequest = 1000
            });
        }
    }
}
