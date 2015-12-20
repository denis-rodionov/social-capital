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
using SocialCapital.Data.Managers;
using System.Linq;

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

			// view registration
			var viewFactory = Container.Get<IViewFactory> ();
			viewFactory.Register<TagsVM, TagsSelectPage> ();
			viewFactory.Register<DeleteContactsVM, DeleteContactsPage> ();

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

		// just in case
		void RemoveDuplicates(IDataContext db)
		{
			var contactManager = App.Container.Get<ContactManager> ();
			var emailManager= App.Container.Get<EmailManager> ();
			var phoneManager = App.Container.Get<PhonesManager> ();

			var groups = contactManager.AllContacts.GroupBy (
				c => c.DisplayName,
				c => c,
				(key, g) => new {
					Name = key,
					Contacts = g.ToList ()
				}).Where (g => g.Contacts.Count () > 1).ToList ();


			foreach (var g in groups)
			{
				var saveOne = g.Contacts.First ();

				foreach (var c in g.Contacts)
					if (c != saveOne)
					{
						var emails = emailManager.GetContactEmails (c.Id);
						var phones = phoneManager.GetContactPhones (c.Id);

						db.Connection.Delete (c);

						foreach (var e in emails)
							db.Connection.Delete (e);
						foreach (var p in phones)
							db.Connection.Delete (p);
					}
			}
		}

		#endregion
	}
}

