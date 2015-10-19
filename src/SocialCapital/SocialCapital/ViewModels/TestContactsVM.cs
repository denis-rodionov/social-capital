using System;
using SocialCapital.Services.AddressBookImport;
using System.Collections.Generic;
using System.Linq;

namespace SocialCapital.ViewModels
{
	public class TestContactsVM : ViewModelBase
	{
		public TestContactsVM (IEnumerable<AddressBookContact> contacts)
		{
			this.contacts = contacts;
		}

		/// <summary>
		/// Contact list
		/// </summary>
		IEnumerable<AddressBookContact> contacts;
		public IEnumerable<AddressBookContact> AllContacts{
			get { return contacts; }
		}

		public IEnumerable<AddressBookContact> FilteredContacts { 
			get { 
				var list = contacts.Where (c => c.DebugString.ToLowerInvariant ().Contains (Filter.ToLowerInvariant ())).ToList ();
				count = list.Count ();
				OnPropertyChanged ("ContactsCount");
				return list;
			}
			set { SetProperty (ref contacts, value); }
		}

		int count = 0;
		public int ContactsCount {
			get { return count; }
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
	}
}

