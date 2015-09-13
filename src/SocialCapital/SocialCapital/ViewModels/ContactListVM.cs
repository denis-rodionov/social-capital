using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using SocialCapital.Data;
using Xamarin.Forms;
using SocialCapital.AddressBookImport;

namespace SocialCapital.ViewModels
{
	public class ContactListVM : ViewModelBase
	{
		/// <summary>
		/// Contact list
		/// </summary>
		IEnumerable<ContactVM> contacts;
		public IEnumerable<ContactVM> Contacts { 
			get { return contacts.Where(c => c.SourceContact.DisplayName.Contains(Filter)); }
			set { SetProperty (ref contacts, value); }
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
			var manager = new ContactManager ();
			var contacts = manager.Contacts .ToList ();

			Contacts = new ObservableCollection<ContactVM> (contacts.Select(c => new ContactVM(c)));
		}


	}
}

