using System;
using System.Reflection;
using Xamarin.Forms;
using SocialCapital.Data;
using SocialCapital.Views;
using SocialCapital.Logging;
using Ninject;
using SocialCapital.ViewModels;
using SocialCapital.Common.FormsMVVM;
using SocialCapital.Services;
using SocialCapital.Services.DropboxSync;
using SocialCapital.Common.EventProviders;

namespace SocialCapital 
{
	public partial class App : Application
	{
		public static StandardKernel Container { get; set; }

		public App ()
		{
			Container = new StandardKernel (
				new DatabaseNinjectModule(), 
				new ViewModelNinjectModule(), 
				new ViewNInjectModule(),
				new MvvmPattenNinjectModule(),
				new ServiceNInjectModule()
			);

			InitializeComponent();
			//Log.GetLogger ().Log ("Application starting......");

			if (Device.OS != TargetPlatform.WinPhone) {
				AppResources.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
			}

			//Container.Get<DatabaseService>().ClearDatabase ();
			Container.Get<DatabaseService>().InitDatabase ();

			RunBackgroundServices ();

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

		#region Implementation

		void RunBackgroundServices()
		{			
			Container.Get<DropboxBackupService> ();
		}

		#endregion
	}
}

