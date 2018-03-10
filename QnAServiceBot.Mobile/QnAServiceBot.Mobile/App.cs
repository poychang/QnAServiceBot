using Xamarin.Forms;

namespace QnAServiceBot.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            MainPage = new NavigationPage(new Pages.LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
