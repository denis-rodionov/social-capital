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
			var timing = Timing.Start ("Get contacts list");

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

			timing.Finish ();

			return contactList;
		}

		#region Load Emails

		private List<Email> LoadEmails(Activity activity, IEnumerable<AddressBookContact> contacts = null, string filterContactId = null)
		{
			if ((contacts != null && filterContactId != null) || (contacts == null && filterContactId == null))
				throw new ArgumentException ("Only one of values must be set (contacts or filterContactId)");

			var loader = new CursorLoader (
				activity,
				ContactsContract.CommonDataKinds.Email.ContentUri, 
				null,	// projection
				filterContactId == null ? null : ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + "=" + filterContactId,
				null,
				null);
			var emailCursor = (ICursor)loader.LoadInBackground ();

			var res = new List<Email> ();

			while (emailCursor.MoveToNext ()) 
			{
				var email = new Email ();
				email.Address = emailCursor.GetString (emailCursor.GetColumnIndex (ContactsContract.CommonDataKinds.Email.Address));
				string contactId = emailCursor.GetString (emailCursor.GetColumnIndex (ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId));

				EmailDataKind pkind = (EmailDataKind) emailCursor.GetInt (emailCursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Type));
				email.Type = ToEmailType(pkind);

				if (pkind != EmailDataKind.Custom)
					email.Label = ContactsContract.CommonDataKinds.Email.GetTypeLabel (activity.Resources, pkind, String.Empty);
				else
					email.Label = emailCursor.GetString (emailCursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Label));

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

		private List<Phone> LoadPhones(Activity activity, IEnumerable<AddressBookContact> contacts = null, string filterContactId = null)
		{
			if ((contacts != null && filterContactId != null) || (contacts == null && filterContactId == null))
				throw new ArgumentException ("Only one of values must be set (contacts or filterContactId)");

			var loader = new CursorLoader (
				activity,
				ContactsContract.CommonDataKinds.Phone.ContentUri, 
				null,	// projection
				filterContactId == null ? null : ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + "=" + filterContactId,
				null,
				null);
			var phonesCursor = (ICursor)loader.LoadInBackground ();

			var res = new List<Phone> ();

			while (phonesCursor.MoveToNext ()) 
			{
				var phone = new Phone ();
				phone.Number = phonesCursor.GetString (phonesCursor.GetColumnIndex (ContactsContract.CommonDataKinds.Phone.Number));
				string contactId = phonesCursor.GetString (phonesCursor.GetColumnIndex (ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId));

				PhoneDataKind pkind = (PhoneDataKind) phonesCursor.GetInt (phonesCursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Type));
				phone.Type = ToPhoneType(pkind);

				if (pkind != PhoneDataKind.Custom)
					phone.Label = ContactsContract.CommonDataKinds.Phone.GetTypeLabel (activity.Resources, pkind, String.Empty);
				else
					phone.Label = phonesCursor.GetString (phonesCursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Label));

				if (contacts != null)
					BindToContact (contactId, phone, contacts, "Phones");

				res.Add (phone);
			}

			phonesCursor.Close ();

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
	}
}

