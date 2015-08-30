using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SocialCapital
{
	public class ContactListViewModel
	{
		public ObservableCollection<Contact> Contacts { get; set; }
			
		public ContactListViewModel ()
		{
			Log.GetLogger ().Log ("==== ContactListViewModel constructor ===");
			var manager = new ContactManager ();
			var contacts = manager.GetContactListPreview ();

			Contacts = new ObservableCollection<Contact> (contacts);
		}
	}
}

