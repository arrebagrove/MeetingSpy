using Xamarin.Forms;

namespace MeetingSpy
{
	public partial class App : Application
	{
		public App()
		{
			MainPage = new NavigationPage(new MeetingSpyPage())
			{
			};
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

