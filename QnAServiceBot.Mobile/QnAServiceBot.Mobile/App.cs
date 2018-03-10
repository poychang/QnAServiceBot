using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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
            AppCenter.Start("ios=8b143315-ec9d-4554-90a3-6491256cafa6;" +
                            "uwp={Your UWP App secret here};" +
                            "android={Your Android App secret here}",
                typeof(Analytics), typeof(Crashes));
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
