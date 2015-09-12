using System;
using System.Collections.Generic;
using SocialCapital.AddressBookImport;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Threading;
using SocialCapital.Data;
using SocialCapital.Data.Model;

namespace SocialCapital.ViewModels
{
	/// <summary>
	/// AddressBook state vime model class
	/// </summary>
	public class AddressBookVM : ViewModelBase
	{
		DateTime? lastImportTime = null;

		#region Properties

		public ICommand StartImport { get; private set; }

		// TODO: add threadin safety here
		bool isImportRunning = false;
		public bool IsImportRunning {
			get { return isImportRunning; }
			set { SetProperty (ref isImportRunning, value); }
		}

		public string Status {
			get { 
				if (lastImportTime.HasValue)
					return string.Format ("{0}: {1}", AppResources.AddressBookSynchStatus, lastImportTime);
				else
					return AppResources.AddressBookNoSynchStatus;
			}
		}

		public ObservableCollection<ContactGroup<DateTime>> ContactGroups { get; private set; }

		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public AddressBookVM ()
		{
			lastImportTime = new Settings ().LastAddressBookImportTime;
			//var t = Test();
			StartImport = new Command (Import);

			ContactGroups = new ObservableCollection<ContactGroup<DateTime>> (
				new ContactManager ().GetContactGroups (c => c.AddressBookUpdateTime, c => c.AddressBookUpdateTime != null)
			);
		}

		#region Implementation

		private async void Import()
		{
			if (IsImportRunning) {
				Log.GetLogger ().Log ("Import is already running...", LogLevel.Error);
				return;
			}

			IsImportRunning = true;
			Log.GetLogger ().Log ("Starting import task...");

			var imported = await Task.Run<ContactGroup<DateTime>>(() => 
				{
					Log.GetLogger ().Log ("Import started");
					var service = new AddressBookService();

					service.LoadContacts();
					var res = service.FullUpdate();
					Log.GetLogger().Log("Import finished");

					return res;
				});
			
			ImportFinished (imported);

			Log.GetLogger().Log("Import task exited");
			IsImportRunning = false;
		}

		private void ImportFinished(ContactGroup<DateTime> importResult)
		{
			UpdateStatus ();

			ContactGroups.Insert (0, importResult);
		}

		void UpdateStatus ()
		{
			lastImportTime = DateTime.Now;
			new Settings ().LastAddressBookImportTime = lastImportTime;
			OnPropertyChanged ("Status");
		}

		#endregion
	}
}

