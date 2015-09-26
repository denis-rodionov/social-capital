using System;
using System.Collections.Generic;
using SocialCapital.Data.Model;
using Ninject;
using SocialCapital.Data;
using System.Linq;

namespace SocialCapital.ViewModels
{
	public class ContactsProcessingVM
	{
		public ContactsProcessingVM ()
		{
			AllContacts = App.Container.Get<ContactManager> ().Contacts;
		}

		public IEnumerable<Contact> AllContacts { get; set; }

		public int TotalContactCount {
			get { return AllContacts.Count(); }
		}

		public int GroupedContactCount {
			get { return AllContacts.Count (c => c.GroupId != 0); }
		}
	}
}

