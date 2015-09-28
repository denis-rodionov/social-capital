using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using SocialCapital.Data;
using Xamarin.Forms;
using SocialCapital.AddressBookImport;
using SocialCapital.Common;
using System.Windows.Input;
using Ninject;

namespace SocialCapital.ViewModels
{
	public class ContactListVM : ViewModelBase
	{
		/// <summary>
		/// Contact list
		/// </summary>
		IEnumerable<ContactVM> contacts;
		public IEnumerable<ContactVM> AllContacts{
			get { return contacts; }
		}

		public IEnumerable<ContactVM> FilteredContacts { 
			get { return contacts.Where(c => c.SourceContact.DisplayName.ToLowerInvariant().Contains(Filter.ToLowerInvariant())); }
			set { SetProperty (ref contacts, value); }
		}

		public int ContactsCount {
			get { return FilteredContacts.Count (); }
		}

		/// <summary>
		/// String for filtering contact list
		/// </summary>
		string filter = "";
		public string Filter {
			get { return filter; }
			set { 
				SetProperty (ref filter, value); 
				OnPropertyChanged ("FilteredContacts");
			}
		}

		/// <summary>
		/// Number of contacts selected in multy-select list
		/// </summary>
		public int SelectedCount {
			get { return contacts.Where (c => c.Selected).Count(); }
		}
			
		public ContactListVM ()
		{
			var timing = Timing.Start ("ContactListVM constructor");

			var manager = App.Container.Get<ContactManager> ();
			var contacts = manager.Contacts .ToList ();

			FilteredContacts = new ObservableCollection<ContactVM> (contacts.Select(c => new ContactVM(c)));
			SelectCommand = new Command (OnSelectCommandExecuted);

			timing.Finish (LogLevel.Trace);
		}

		public ICommand SelectCommand { get; set; }
		private void OnSelectCommandExecuted(object item)
		{
			var contact = (ContactVM)item;
			contact.Selected = !contact.Selected;
			OnPropertyChanged ("SelectedCount");
		}


	}
}

