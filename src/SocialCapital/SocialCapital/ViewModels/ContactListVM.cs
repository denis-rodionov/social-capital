using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using SocialCapital.Data;
using Xamarin.Forms;
using SocialCapital.AddressBookImport;
using SocialCapital.Common;

namespace SocialCapital.ViewModels
{
	public class ContactListVM : ViewModelBase
	{
		/// <summary>
		/// Contact list
		/// </summary>
		IEnumerable<ContactVM> contacts;
		public IEnumerable<ContactVM> Contacts { 
			get { return contacts.Where(c => c.SourceContact.DisplayName.ToLowerInvariant().Contains(Filter.ToLowerInvariant())); }
			set { SetProperty (ref contacts, value); }
		}

		public int ContactsCount {
			get { return Contacts.Count (); }
		}

		/// <summary>
		/// String for filtering contact list
		/// </summary>
		string filter = "";
		public string Filter {
			get { return filter; }
			set { 
				SetProperty (ref filter, value); 
				OnPropertyChanged ("Contacts");
			}
		}
			
		public ContactListVM ()
		{
			var timing = Timing.Start ("ContactListVM constructor");

			var manager = new ContactManager ();
			var contacts = manager.Contacts .ToList ();

			Contacts = new ObservableCollection<ContactVM> (contacts.Select(c => new ContactVM(c)));

			timing.Finish (LogLevel.Trace);
		}


	}
}

