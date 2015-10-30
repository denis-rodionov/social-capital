using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;

namespace SocialCapital.Droid
{
	[Activity (Label = "@string/app_name", 
			   Icon = "@drawable/icon", 
			   MainLauncher = true, 
			   ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity :  global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			//Log.GetLogger ().Log ("Android application starting...");

			// https://forums.xamarin.com/discussion/13784/catching-global-exceptions-in-android
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App ());
		}

		private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			Log.GetLogger ().Log ("!Unhandled exception catched:", LogLevel.Critical);
			Log.GetLogger ().Log (args.ExceptionObject as Exception);
		}
	}
}

