using Newtonsoft.Json;
using QnAServiceBot.Mobile.Models;
using QnAServiceBot.Mobile.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QnAServiceBot.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgentPage : ContentPage
    {
        private BotService _botService;

        public AgentPage(UserModel user)
        {
            InitializeComponent();

            CurrentUser = user;

            ConversationList = new ObservableCollection<Activity>();
            BindingContext = this;
            _botService = new BotService();
        }

        public ObservableCollection<Activity> ConversationList { get; }
        private UserModel CurrentUser { get; }
        private BotTokenModel BotToken { get; set; }

        public async void SendButtonClicked(object sender, EventArgs e)
        {
            var botResponse = await _botService.SendMessage(CurrentUser, Message.Text);
            ConversationList.Add(botResponse);
            CheckMessage.Text = JsonConvert.SerializeObject(botResponse);
        }

        public async void ConnectButtonClicked(object sender, EventArgs e)
        {
            var botResponse = await _botService.SendConnect(CurrentUser, false);
            ConversationList.Add(botResponse);
            CheckMessage.Text = JsonConvert.SerializeObject(botResponse);
        }

        public async void StopButtonClicked(object sender, EventArgs e)
        {
            var botResponse = await _botService.SendStopConversation(CurrentUser);
            ConversationList.Add(botResponse);
            CheckMessage.Text = JsonConvert.SerializeObject(botResponse);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            BotToken = await _botService.FetchBotToken(CurrentUser, CancellationToken.None);
            await _botService.Setup(BotToken.Token);
            CheckMessage.Text = BotToken.Token;
        }
    }
}
