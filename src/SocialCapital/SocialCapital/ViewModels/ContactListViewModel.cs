using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using SocialCapital.Data;

namespace SocialCapital.ViewModels
{
	public class ContactListViewModel
	{
		public ObservableCollection<Contact> Contacts { get; set; }

		public ObservableCollection<Tag> Tags { get; set; }
			
		public ContactListViewModel ()
		{
			Log.GetLogger ().Log ("==== ContactListViewModel constructor ===");
			var manager = new ContactManager ();
			var contacts = manager.GetContactListPreview ();

			Contacts = new ObservableCollection<Contact> (contacts);
			Tags = new ObservableCollection<Tag>() {new Tag() { Name = "Осень" }, new Tag() { Name = "Зима" }};
		}
	}
}

