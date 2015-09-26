using System;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;

namespace SocialCapital.ViewModels
{
	public class ContactGroupVM
	{
		public ContactGroupVM ()
		{
			AssignedContacts = new ObservableCollection<Contact> ();
		}

		public string Name { get; set; }

		public ObservableCollection<Contact> AssignedContacts { get; set; }
	}
}

