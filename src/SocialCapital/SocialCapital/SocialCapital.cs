using System;

using Xamarin.Forms;

namespace SocialCapital 
{
	public class App : Application
	{
		//public DataContext context {get; set;}

		public App ()
		{
			Log.GetLogger ().Log ("Application starting...");

			new ContactManager ().Init ();

			MainPage = new NavigationPage (new ContactListPage ());
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

