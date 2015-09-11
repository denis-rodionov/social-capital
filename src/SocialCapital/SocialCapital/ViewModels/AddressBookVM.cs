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

namespace SocialCapital.ViewModels
{
	public class AbContactGroup : IEnumerable<AddressBookContact>
	{
		public string GroupName { get; set; }
		public List<AddressBookContact> Contacts { get; set; }

		#region IEnumerable implementation

		IEnumerator<AddressBookContact> IEnumerable<AddressBookContact>.GetEnumerator ()
		{
			return Contacts.GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return Contacts.GetEnumerator ();
		}

		#endregion
	}

	public class AddressBookVM : ViewModelBase
	{
		public ICommand StartImport { get; private set; }

		DateTime? lastImportTime = null;

		/// <summary>
		/// Constructor
		/// </summary>
		public AddressBookVM ()
		{
			lastImportTime = new Settings ().LastAddressBookImportTime;
			StartImport = new Command (Import);
		}

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

		public IEnumerable<AbContactGroup> ContactsGroups { get; set; }

		public ObservableCollection<AbContactGroup> ContactGroups { get; set; }

		#region Implementation

		private async void Import()
		{
			if (IsImportRunning) {
				Log.GetLogger ().Log ("Import is already running...", LogLevel.Error);
				return;
			}

			IsImportRunning = true;
			Log.GetLogger ().Log ("Starting import task...");

			await Task.Factory.StartNew (() => {
				Log.GetLogger ().Log ("Import started");
				var service = new AddressBookService();
				service.LoadContacts();
				service.FullUpdate();
				Log.GetLogger().Log("Import finished");
			});

			ImportFinished ();

			Log.GetLogger().Log("Import task exited");
			IsImportRunning = false;
		}

		private void ImportFinished()
		{
			lastImportTime = DateTime.Now;
			OnPropertyChanged ("Status");
		}

		#endregion
	}
}

