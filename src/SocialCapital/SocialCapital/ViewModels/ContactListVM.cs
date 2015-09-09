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

			//Contacts = new ObservableCollection<ContactVM> (contacts.Select(c => new ContactVM(c)));

			var service = DependencyService.Get<IAddressBookInformation> ();
			var abContacts = service.GetContacts ().Result;
			int count = abContacts.Count;

			LogStatistics (abContacts, count);

			int number = 0;
			Contacts = new ObservableCollection<ContactVM>(abContacts.Select (ab => 
				new ContactVM(new Contact () {
					Id = ++number,
					FullName = ab.DisplayName,
					AbContact = ab,
					Photo = ab.Thumbnail
				})));
		}

		static void LogStatistics (List<AddressBookContact> abContacts, int count)
		{
			Log.GetLogger ().Log ("FirstName : {0} from {1} ({2}%)", abContacts.Where (c => c.FirstName != null).Count (), count, (int)(abContacts.Where (c => c.FirstName != null).Count () * 100 / count));
			Log.GetLogger ().Log ("LastName : {0} from {1} ({2}%)", abContacts.Where (c => c.LastName != null).Count (), count, (int)(abContacts.Where (c => c.LastName != null).Count () * 100 / count));
			Log.GetLogger ().Log ("MiddleName : {0} from {1} ({2}%)", abContacts.Where (c => c.MiddleName != null).Count (), count, (int)(abContacts.Where (c => c.MiddleName != null).Count () * 100 / count));
			Log.GetLogger ().Log ("DisplayName : {0} from {1} ({2}%)", abContacts.Where (c => c.DisplayName != null).Count (), count, (int)(abContacts.Where (c => c.DisplayName != null).Count () * 100 / count));
			Log.GetLogger ().Log ("NickName : {0} from {1} ({2}%)", abContacts.Where (c => c.NickName != null).Count (), count, (int)(abContacts.Where (c => c.NickName != null).Count () * 100 / count));
			Log.GetLogger ().Log ("Prefix : {0} from {1} ({2}%)", abContacts.Where (c => c.Prefix != null).Count (), count, (int)(abContacts.Where (c => c.Prefix != null).Count () * 100 / count));
			Log.GetLogger ().Log ("Suffix : {0} from {1} ({2}%)", abContacts.Where (c => c.Suffix != null).Count (), count, (int)(abContacts.Where (c => c.Suffix != null).Count () * 100 / count));
			Log.GetLogger ().Log ("IsAggregate=true : {0} from {1} ({2}%)", abContacts.Where (c => c.IsAggregate == true).Count (), count, (int)(abContacts.Where (c => c.IsAggregate == true).Count () * 100 / count));
			Log.GetLogger ().Log ("Thumbnail : {0} from {1} ({2}%)", abContacts.Where (c => c.Thumbnail != null).Count (), count, (int)(abContacts.Where (c => c.FirstName != null).Count () * 100 / count));
			Log.GetLogger ().Log ("Organizations : {0} from {1} ({2}%)", abContacts.Where (c => c.Organizations.Count () != 0).Count (), count, (int)(abContacts.Where (c => c.Organizations.Count () != 0).Count () * 100 / count));
			Log.GetLogger ().Log ("Phones : {0} from {1} ({2}%)", abContacts.Where (c => c.Phones.Count () != 0).Count (), count, (int)(abContacts.Where (c => c.Phones.Count () != 0).Count () * 100 / count));
			Log.GetLogger ().Log ("Emails : {0} from {1} ({2}%)", abContacts.Where (c => c.Emails.Count () != 0).Count (), count, (int)(abContacts.Where (c => c.Emails.Count () != 0).Count () * 100 / count));
			Log.GetLogger ().Log ("Notes : {0} from {1} ({2}%)", abContacts.Where (c => c.Notes.Count () != 0).Count (), count, (int)(abContacts.Where (c => c.Notes.Count () != 0).Count () * 100 / count));
		}
	}
}

