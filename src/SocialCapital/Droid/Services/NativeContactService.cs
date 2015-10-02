using System;
using SocialCapital.AddressBookImport;
using System.Collections.Generic;
using Android.Database;
using Android.App;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Android.Content;
using Xamarin.Forms;
using SocialCapital.Data.Model;
using SocialCapital.Common;
using System.Linq;

[assembly: Dependency(typeof(SocialCapital.Droid.Services.NativeContactService))]

namespace SocialCapital.Droid.Services
{
	public class NativeContactService : BaseAddressBookService, IAddressBookInformation
	{
		public NativeContactService ()
		{
		}

		#region interface IAddressBookInformation

		public IEnumerable<AddressBookContact> GetContacts ()
		{
			return GetAllContacts ();
		}

		#endregion

		// Populate the contact list based on account currently selected in the account spinner.
		private List<AddressBookContact> GetAllContacts ()
		{
			var activity = Forms.Context as Activity;

			// Build adapter with contact entries
			var uri = ContactsContract.Contacts.ContentUri;

			string[] projection = {
				ContactsContract.Contacts.InterfaceConsts.Id,
				ContactsContract.Contacts.InterfaceConsts.DisplayName,
				ContactsContract.Contacts.InterfaceConsts.PhotoId,
				ContactsContract.Contacts.InterfaceConsts.ContactChatCapability,
				ContactsContract.Contacts.InterfaceConsts.ContactLastUpdatedTimestamp,
				ContactsContract.Contacts.InterfaceConsts.ContactPresence,
				ContactsContract.Contacts.InterfaceConsts.ContactStatus,
				ContactsContract.Contacts.InterfaceConsts.ContactStatusIcon,
				ContactsContract.Contacts.InterfaceConsts.ContactStatusLabel,
				ContactsContract.Contacts.InterfaceConsts.ContactStatusResPackage,
				ContactsContract.Contacts.InterfaceConsts.ContactStatusTimestamp,
				//ContactsContract.Contacts.InterfaceConsts.Count,
				ContactsContract.Contacts.InterfaceConsts.CustomRingtone,
				ContactsContract.Contacts.InterfaceConsts.DisplayName,
				ContactsContract.Contacts.InterfaceConsts.DisplayNameAlternative,
				ContactsContract.Contacts.InterfaceConsts.DisplayNamePrimary,
				ContactsContract.Contacts.InterfaceConsts.DisplayNameSource,
				ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber,
				ContactsContract.Contacts.InterfaceConsts.InVisibleGroup,
				ContactsContract.Contacts.InterfaceConsts.IsUserProfile,
				ContactsContract.Contacts.InterfaceConsts.LastTimeContacted,
				ContactsContract.Contacts.InterfaceConsts.LookupKey,
				ContactsContract.Contacts.InterfaceConsts.PhoneticName,
				ContactsContract.Contacts.InterfaceConsts.PhoneticNameStyle,
				ContactsContract.Contacts.InterfaceConsts.PhotoFileId,
				ContactsContract.Contacts.InterfaceConsts.PhotoThumbnailUri,
				ContactsContract.Contacts.InterfaceConsts.PhotoUri,
				ContactsContract.Contacts.InterfaceConsts.SendToVoicemail,
				ContactsContract.Contacts.InterfaceConsts.SortKeyAlternative,
				ContactsContract.Contacts.InterfaceConsts.SortKeyPrimary,
				ContactsContract.Contacts.InterfaceConsts.Starred,
				ContactsContract.Contacts.InterfaceConsts.TimesContacted
				//ContactsContract.CommonDataKinds.Phone.Number
			};

			var timing = Timing.Start ("Get contacts list");

			// CursorLoader introduced in Honeycomb (3.0, API11)
			var loader = new CursorLoader(activity, uri, projection, null, null, null);
			var cursor = (ICursor)loader.LoadInBackground();

			var contactList = new List<AddressBookContact> ();  
			if (cursor.MoveToFirst ()) {
				do {
					
					var newContact = new AddressBookContact() {
						Id = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.Id)),
						DisplayName = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.DisplayName)),
						LastUpdatedTimespamp = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.ContactLastUpdatedTimestamp)),
						HasPhoneNumber = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber)),
						//InDefaultDirectory = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.D)),
						InVisibleGroup = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.InVisibleGroup)),
						LookupKey = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.LookupKey)),
						RawContactId = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.TimesContacted)),
						PhotoFileId = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.PhotoFileId)),
						PhotoId = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.PhotoId)),
						ThumbnailUri = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.PhotoThumbnailUri)),
						PhotoUri = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.PhotoUri)),
						Stared = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.Starred)),
						ContactStatusTimestamp = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.ContactStatusTimestamp))
					};

					if (newContact.HasPhoneNumber == "1")
					{
//						var newPhone = new Phone();
//						newPhone.Number = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Phone.Number));
//						newContact.Phones = new List<Phone> { newPhone };
					}

					contactList.Add (newContact);

				} while (cursor.MoveToNext());
			}

			LoadPhones(activity, contactList);
			LoadEmails (activity, contactList);
			LoadExtras (activity, contactList);

			timing.Finish ();

			return contactList;
		}

		private ICursor GetCursor(Android.Net.Uri uri, Activity activity, IEnumerable<AddressBookContact> contacts = null, AddressBookContact forContact = null)
		{
			if ((contacts != null && forContact != null) || (contacts == null && forContact == null))
				throw new ArgumentException ("Only one of values must be set (contacts or filterContactId)");

			var loader = new CursorLoader (
				activity,
				uri, 
				null,	// projection
				forContact == null ? null : "contact_id=" + forContact.Id,
				null,
				null);
			return (ICursor)loader.LoadInBackground ();
		}

		private void BindToContact<T>(string contactId, T phone, IEnumerable<AddressBookContact> contacts, string propertyName)
		{
			var contact = contacts.SingleOrDefault (c => c.Id == contactId);

			if (contact != null)
			{
				var list = contact.GetType ().GetProperty (propertyName).GetValue (contact);
				var tList = (List<T>)list;
				tList.Add (phone);
			}
			else
				Log.GetLogger ().Log (string.Format ("Cannot find contact with id = {0}", contactId));
		}

		private void Bind<T>(string propertyName, T item, IEnumerable<AddressBookContact> contacts, string androidContactId, AddressBookContact forContact)
		{
			if (contacts != null)
				BindToContact (androidContactId, item, contacts, propertyName);
			else
			{
				var list = (List<T>)forContact.GetType ().GetProperty (propertyName).GetValue (forContact);
				list.Add (item);
			}
		}

		private void LoadExtras(Activity activity, IEnumerable<AddressBookContact> contacts = null, AddressBookContact forContact = null)
		{
			var cursor = GetCursor (ContactsContract.Data.ContentUri, activity, contacts, forContact);
			var res = new List<Organization> ();

			while (cursor.MoveToNext ()) 
			{
				string dataType = cursor.GetString (cursor.GetColumnIndex (ContactsContract.DataColumns.Mimetype));
				string contactId = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Organization.InterfaceConsts.ContactId));

				switch (dataType)
				{
					case ContactsContract.CommonDataKinds.Organization.ContentItemType:
						var org = GetOrganization (cursor, activity);
						Bind ("Organizations", org, contacts, contactId, forContact);
						break;
					case ContactsContract.CommonDataKinds.StructuredPostal.ContentItemType:
						var address = GetAddress (cursor, activity);
						Bind ("Addresses", address, contacts, contactId, forContact);
						break;	
					case ContactsContract.CommonDataKinds.Note.ContentItemType:
						var note = GetNote (cursor);
						Bind ("Notes", note, contacts, contactId, forContact);
						break;
				}
			}
		}

		#region Load Emails

		private List<Email> LoadEmails(Activity activity, IEnumerable<AddressBookContact> contacts = null, AddressBookContact forContact = null)
		{
			var cursor = GetCursor (ContactsContract.CommonDataKinds.Email.ContentUri, activity, contacts, forContact);
			var res = new List<Email> ();

			while (cursor.MoveToNext ()) 
			{
				var email = new Email ();
				email.Address = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Email.Address));
				string contactId = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId));

				EmailDataKind pkind = (EmailDataKind) cursor.GetInt (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Type));
				email.Type = ToEmailType(pkind);

				if (pkind != EmailDataKind.Custom)
					email.Label = ContactsContract.CommonDataKinds.Email.GetTypeLabel (activity.Resources, pkind, String.Empty);
				else
					email.Label = cursor.GetString (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Label));

				if (contacts != null)
					BindToContact (contactId, email, contacts, "Emails");

				res.Add (email);
			}

			return res;
		}

		private EmailType ToEmailType (EmailDataKind emailKind)
		{
			switch (emailKind)
			{
				case EmailDataKind.Home:
					return EmailType.Home;
				case EmailDataKind.Work:
					return EmailType.Work;
				default:
					return EmailType.Other;
			}
		}

		#endregion

		#region Load phones

		private List<Phone> LoadPhones(Activity activity, IEnumerable<AddressBookContact> contacts = null, AddressBookContact forContact = null)
		{
			var cursor = GetCursor (ContactsContract.CommonDataKinds.Phone.ContentUri, activity, contacts, forContact);
			var res = new List<Phone> ();

			while (cursor.MoveToNext ()) 
			{
				var phone = new Phone ();
				phone.Number = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Phone.Number));
				string contactId = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId));

				PhoneDataKind pkind = (PhoneDataKind) cursor.GetInt (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Type));
				phone.Type = ToPhoneType(pkind);

				if (pkind != PhoneDataKind.Custom)
					phone.Label = ContactsContract.CommonDataKinds.Phone.GetTypeLabel (activity.Resources, pkind, String.Empty);
				else
					phone.Label = cursor.GetString (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Label));

				if (contacts != null)
					BindToContact (contactId, phone, contacts, "Phones");

				res.Add (phone);
			}

			cursor.Close ();

			return res;
		}

		internal static PhoneType ToPhoneType (PhoneDataKind phoneKind)
		{
			switch (phoneKind)
			{
				case PhoneDataKind.Home:
					return PhoneType.Home;
				case PhoneDataKind.Mobile:
					return PhoneType.Mobile;
				case PhoneDataKind.FaxHome:
					return PhoneType.HomeFax;
				case PhoneDataKind.Work:
					return PhoneType.Work;
				case PhoneDataKind.FaxWork:
					return PhoneType.WorkFax;
				case PhoneDataKind.Pager:
				case PhoneDataKind.WorkPager:
					return PhoneType.Pager;
				default:
					return PhoneType.Other;
			}
		}

		#endregion



		#region Load Organizations

		private Organization GetOrganization(ICursor cursor, Activity activity)
		{
			var company = new Organization ();
			company.Name = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Organization.Company));
			company.ContactTitle = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Organization.Title));

			OrganizationDataKind pkind = (OrganizationDataKind) cursor.GetInt (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Type));

			if (pkind != OrganizationDataKind.Custom)
				company.Label = ContactsContract.CommonDataKinds.Organization.GetTypeLabel (activity.Resources, pkind, String.Empty);
			else
				company.Label = cursor.GetString (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Label));

			return company;
		}

//		private List<Organization> LoadOrganizations(Activity activity, IEnumerable<AddressBookContact> contacts = null, string filterContactId = null)
//		{
//			var cursor = GetCursor (ContactsContract.Data.ContentUri, activity, contacts, filterContactId);
//			var res = new List<Organization> ();
//
//			while (cursor.MoveToNext ()) 
//			{
//				var company = new Organization ();
//				company.Name = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Organization.Company));
//				company.ContactTitle = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Organization.Title));
//
//				string contactId = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Organization.InterfaceConsts.ContactId));
//
//				OrganizationDataKind pkind = (OrganizationDataKind) cursor.GetInt (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Type));
//
//				if (pkind != OrganizationDataKind.Custom)
//					company.Label = ContactsContract.CommonDataKinds.Organization.GetTypeLabel (activity.Resources, pkind, String.Empty);
//				else
//					company.Label = cursor.GetString (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Label));
//
//				if (contacts != null)
//					BindToContact (contactId, company, contacts, "Organizations");
//
//				res.Add (company);
//			}
//
//			cursor.Close ();
//
//			return res;
//		}

		#endregion

		#region Load Addresses

		private Address GetAddress(ICursor cursor, Activity activity)
		{
			var address = new Address ();
			address.Country = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.Country));
			address.Region = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.Region));
			address.City = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.City));
			address.PostalCode = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.Postcode));
			address.StreetAddress = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.Street));

			var pkind = (AddressDataKind) cursor.GetInt (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Type));

			if (pkind != AddressDataKind.Custom)
				address.Label = ContactsContract.CommonDataKinds.StructuredPostal.GetTypeLabel (activity.Resources, pkind, String.Empty);
			else
				address.Label = cursor.GetString (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Label));

			return address;
		}

//		private List<Address> LoadAddresses(Activity activity, IEnumerable<AddressBookContact> contacts = null, string filterContactId = null)
//		{
//			var cursor = GetCursor (ContactsContract.Data.ContentUri, activity, contacts, filterContactId);
//			var res = new List<Address> ();
//
//			while (cursor.MoveToNext ()) 
//			{
//				var address = new Address ();
//				address.Country = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.Country));
//				address.Region = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.Region));
//				address.City = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.City));
//				address.PostalCode = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.Postcode));
//				address.StreetAddress = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.Street));
//
//				string contactId = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.InterfaceConsts.ContactId));
//
//				var pkind = (AddressDataKind) cursor.GetInt (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Type));
//
//				if (pkind != AddressDataKind.Custom)
//					address.Label = ContactsContract.CommonDataKinds.StructuredPostal.GetTypeLabel (activity.Resources, pkind, String.Empty);
//				else
//					address.Label = cursor.GetString (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Label));
//
//				if (contacts != null)
//					BindToContact (contactId, address, contacts, "Addresses");
//
//				res.Add (address);
//			}
//
//			cursor.Close ();
//
//			return res;
//		}

		#endregion

		#region Load Notes

		private Note GetNote(ICursor cursor)
		{
			var note = new Note ();
			note.Contents = cursor.GetString (cursor.GetColumnIndex (ContactsContract.DataColumns.Data1));
			return note;
		}

//		private List<Note> LoadNotes(Activity activity, IEnumerable<AddressBookContact> contacts = null, string filterContactId = null)
//		{
//			var cursor = GetCursor (ContactsContract.Data.ContentUri, activity, contacts, filterContactId);
//			var res = new List<Note> ();
//
//			while (cursor.MoveToNext ()) 
//			{
//				var note = new Note ();
//				note.Contents = cursor.GetString (cursor.GetColumnIndex (ContactsContract.DataColumns.Data1));
//
//				string contactId = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Note.InterfaceConsts.ContactId));
//
//				if (contacts != null)
//					BindToContact (contactId, note, contacts, "Notes");
//
//				res.Add (note);
//			}
//
//			cursor.Close ();
//
//			return res;
//		}

		#endregion


	}
}

