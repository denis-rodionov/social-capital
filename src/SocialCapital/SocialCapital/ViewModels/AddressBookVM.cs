using System;
using System.Collections.Generic;
using SocialCapital.AddressBookImport;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace SocialCapital.ViewModels
{
	public class AbContactGroup : IEnumerable<AddressBookContact>
	{
		public string GroupName { get; set; }
		public List<AddressBookContact> Contacts { get; set; }
	}

	public class AddressBookVM : ViewModelBase
	{
		public ICommand StartImport { get; private set; }

		public AddressBookVM ()
		{
			StartImport = new Command (Import);
		}

		// TODO: add threadin safety here
		bool isImportRunning = false;
		public bool IsImportRunning {
			get { return isImportRunning; }
			set { SetProperty (ref isImportRunning, value); }
		}

		public bool Status {
			get { return ""; }
		}

		public ObservableCollection<AbContactGroup> ContactGroups { get; set; }

		#region Implementation

		private async void Import()
		{
			if (IsImportRunning)
				return;

			IsImportRunning = true;

			await Task.Factory.StartNew (() => {
				int t = 5;	
			});

			IsImportRunning = false;
		}

		#endregion
	}
}

