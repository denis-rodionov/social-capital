using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using Ninject;
using SocialCapital.Data;

namespace SocialCapital.ViewModels
{
	public class ContactGroupListVM : ViewModelBase
	{
		private IEnumerable<Contact> Contacts { get; set; }

		private IEnumerable<Group> Groups { get; set; }

		public ContactGroupListVM ()
		{
			Contacts = App.Container.Get<ContactManager> ().Contacts;
			Groups = App.Container.Get<GroupsManager> ().GetAllGroups (g => true);
		}

		public IEnumerable<Contact> NotGroupedContacts {
			get { return Contacts.Where (c => c.GroupId == 0); }
		}

		public IEnumerable<Group> UsedGroups {
			get { return Groups.Where (g => g.AssignedContacts.Any()); }
		}

		public IEnumerable<Group> UnusedGroup {
			get { return Groups.Where (g => !g.AssignedContacts.Any()); }
		}
	}
}

