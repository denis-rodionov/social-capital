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
	public class ContactListVM
	{
		public ObservableCollection<ContactVM> Contacts { get; set; }
			
		public ContactListVM ()
		{
			var manager = new ContactManager ();
			var contacts = manager.Contacts .ToList ();

			Contacts = new ObservableCollection<ContactVM> (contacts.Select(c => new ContactVM(c)));
		}


	}
}

