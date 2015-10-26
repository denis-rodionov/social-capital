using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SocialCapital.ViewModels;
using SocialCapital.Data;
using SocialCapital.Services.AddressBookImport;

namespace SocialCapital.Views
{
	public partial class SettingsPage : ContentPage
	{
		private SettingsVM Settings { get; set; }
		private DatabaseService databaseService;

		public SettingsPage (SettingsVM settings, DatabaseService databaseService)
		{
			Settings = settings;
			BindingContext = Settings;
			this.databaseService = databaseService;

			InitializeComponent ();
		}

//		private void OnDropboxSwitchToggled(object sender, ToggledEventArgs args)
//		{
//			var vm = (SettingsVM)BindingContext;
//			vm.SyncEnable (args.Value);
//		}

		private void OnShowLogs(object sender, EventArgs args)
		{		
			var vm = new LogViewModel ();
			var page = new LogPage(vm);
			Navigation.PushAsync (page);
		}

		private async void OnErase(object sender, EventArgs args)
		{
			var yes = await DisplayAlert ("Warning", "Are you sure you want to delete all data", "Yes", "No");

			if (yes)
				databaseService.ClearDatabase ();
		}

		private void OnLoadContacts(object sender, EventArgs args)
		{
			var service = DependencyService.Get<IAddressBookInformation> ();

			var contacts = service.GetContacts ();

			var page = new TestContacts (new TestContactsVM (contacts));
			Navigation.PushAsync (page);
		}
	}
}

