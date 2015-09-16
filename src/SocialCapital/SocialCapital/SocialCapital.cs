using System;
using System.Reflection;
using Xamarin.Forms;
using SocialCapital.Data;
using SocialCapital.Views;

namespace SocialCapital 
{
	public class App : Application
	{
		//public static Ioc Ioc { get; set; }

		public App ()
		{

			Log.GetLogger ().Log ("Application starting......");

			if (Device.OS != TargetPlatform.WinPhone) {
				AppResources.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
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

