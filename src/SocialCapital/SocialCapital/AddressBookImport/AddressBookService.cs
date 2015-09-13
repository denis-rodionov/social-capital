﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using SocialCapital.Data.Model;
using SocialCapital.Data;
using SocialCapital.Common;

namespace SocialCapital.AddressBookImport
{
	public class ProgressValue {
		public int ContactsRetrieved { get; set; }
		public int ContactsSync { get; set; }
		public int TotalContacts { get; set; }
	}

	public class AddressBookService
	{
		public IEnumerable<AddressBookContact> LoadedContacts { get; set; }

		public IEnumerable<AddressBookContact> UpdateList { get; set; }

		public IEnumerable<AddressBookContact> NewList { get; set; }

		public event Action<ProgressValue> ProgressEvent;

		ProgressValue CurrentProgressValue { get; set; }

		public AddressBookService ()
		{
			CurrentProgressValue = new ProgressValue ();
		}

		public IEnumerable<AddressBookContact> LoadContacts()
		{
			var timing = Timing.Start ("AddressBookService.LoadContacts");

			var service = DependencyService.Get<IAddressBookInformation> ();

			service.ContactsCountCalculated += (int totalCount) => { 
				CurrentProgressValue.TotalContacts = totalCount; 
			};
			service.ContactRetrieved += (int count) => {
				CurrentProgressValue.ContactsRetrieved = count;
				RaiseProgress(CurrentProgressValue);
			};

			var abContacts = service.GetContacts ();


			timing.Finish ();

			LoadedContacts = abContacts;

			return LoadedContacts;
		}

		/// <summary>
		/// Update all contact in database
		/// </summary>
		/// <returns>The group of updated contacts</returns>
		public ContactGroup<DateTime> FullUpdate()
		{
			if (LoadedContacts == null)
				throw new Exception ("Load contacts first");
			
			var timing = Timing.Start ("AddressBookService.FullUpdate");
			var db = new ContactManager ();
			var updateTime = DateTime.Now;
			var resGroup = new ContactGroup<DateTime> () { GroupName = updateTime, Contacts = new List<Contact>() };

			foreach (var bookContact in LoadedContacts) {
				if (bookContact.Phones.Count () != 0) {
					var dbContact = db.GetContacts (c => c.AddressBookId == bookContact.Id).SingleOrDefault ();
					var savedContact = db.SaveOrUpdateContact (bookContact, updateTime, dbContact);

					(resGroup.Contacts as List<Contact>).Add (savedContact);

					CurrentProgressValue.ContactsSync++;
				}
				RaiseProgress (CurrentProgressValue);
			}

			timing.Finish ();

			return resGroup;
		}

		void RaiseProgress(ProgressValue value)
		{
			if (ProgressEvent != null)
				ProgressEvent (value);
		}

		void LogStatistics (List<AddressBookContact> abContacts, int count)
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

