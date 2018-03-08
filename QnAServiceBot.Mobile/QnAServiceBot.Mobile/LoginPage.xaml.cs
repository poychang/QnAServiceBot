using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using QnAServiceBot.Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QnAServiceBot.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void ButtonLogin_Clicked(object sender, EventArgs e)
        {
            var username = EntryAccount.Text;
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(
                new Uri($@"{Constants.API_URL}/user/login"),
                new StringContent(
                    JsonConvert.SerializeObject(new LoginModel() { Username = username }), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode) return;

            var data = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserModel>(data);
            EntryCheckAccount.Text = data;
        }
    }
}