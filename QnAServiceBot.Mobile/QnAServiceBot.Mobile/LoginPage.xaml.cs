using Newtonsoft.Json;
using QnAServiceBot.Mobile.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
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

            switch (user.Role)
            {
                case "agent":
                    break;

                case "user":
                    await Navigation.PushAsync(new UserPage(user));
                    break;
            }

            ClearStackTo();
            EntryCheckAccount.Text = data;
        }

        /// <summary>清空導航堆疊</summary>
        private void ClearStackTo()
        {
            var stack = Navigation.NavigationStack;

            while (stack.Count > 1)
            {
                var page = stack.First();
                if (page != null)
                {
                    Navigation.RemovePage(page);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
