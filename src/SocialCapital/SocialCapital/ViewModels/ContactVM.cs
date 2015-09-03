using System;
using SocialCapital.Data.Model;
using System.Collections.ObjectModel;

namespace SocialCapital.ViewModels
{
	public class ContactVM 
	{
		public string FullName { get; set; }

		public string WorkPlace { get; set; }

		public ObservableCollection<Tag> Tags { get; set; }

		public ContactVM (Contact contact)
		{
			FullName = contact.FullName;
			WorkPlace = contact.WorkPlace;
			Tags = new ObservableCollection<Tag> (contact.Tags);
		}

		public void Save()
		{
			
		}
	}
}

