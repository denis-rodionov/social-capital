using System;
using System.Reflection;
using Xamarin.Forms;
using SocialCapital.Data;
using SocialCapital.Views;

namespace SocialCapital 
{
	public class App : Application
	{
		//public DataContext context {get; set;}

		public App ()
		{
			Log.GetLogger ().Log ("Application starting......");

			// NOTE: use for debugging, not in released app code!
			var assembly = typeof(App).GetTypeInfo().Assembly;
			foreach (var res in assembly.GetManifestResourceNames())
				System.Diagnostics.Debug.WriteLine("found resource: " + res);

			if (Device.OS != TargetPlatform.WinPhone) {
				AppResources.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
				//Resx.AppResources.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
			}

			new DataContext ().ClearDatabase ();
			new TagManager ().Init ();
			new ContactManager ().Init ();


			//MainPage = new NavigationPage (new ContactListPage ());
			MainPage = new RootPage();
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

