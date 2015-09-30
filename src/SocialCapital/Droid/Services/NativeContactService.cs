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
				ContactsContract.Contacts.InterfaceConsts.TimesContacted,
			};

			// CursorLoader introduced in Honeycomb (3.0, API11)
			var loader = new CursorLoader(activity, uri, projection, null, null, null);
			var cursor = (ICursor)loader.LoadInBackground();

			var contactList = new List<AddressBookContact> ();  
			if (cursor.MoveToFirst ()) {
				do {
					
					contactList.Add (new AddressBookContact{
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
					});

				} while (cursor.MoveToNext());
			}

			timing.Finish ();

			return contactList;
		}

//		List<Phone> LoadPhones(Activity activity)
//		{
//			Android.Net.Uri phoneUri = ContactsContract.CommonDataKinds.Phone.ContentUri;
//
//			string[] projection = null;
//
//			var loader = new CursorLoader (
//				activity,
//				ContactsContract.CommonDataKinds.Phone.ContentUri, 
//				projection,
//				ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + "="+contactId,
//				null,
//				null);
//			var phonesCursor = (ICursor)loader.LoadInBackground ();
//
//			//LogCursorColumns ("Phone cursor", phonesCursor);
//
//			/* // Using Managed Query
//			var phonesCursor = contactsActivity.ManagedQuery (
//				ContactsContract.CommonDataKinds.Phone.ContentUri, 
//				projection,
//				ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + "="+contactId,
//				null,
//				null);
//			*/
//			var res = new List<LocalContactPhone> ();
//
//			while (phonesCursor.MoveToNext ()) 
//			{
//				var phoneCursorIndex = phonesCursor.GetColumnIndex (ContactsContract.CommonDataKinds.Phone.Number);
//				//var phoneCursorIndex = phonesCursor.GetColumnIndex (ContactsContract.CommonDataKinds.Phone.);
//
//				var number = phonesCursor.GetString (phonesCursor.GetColumnIndex (ContactsContract.CommonDataKinds.Phone.Number));
//				//var number = phonesCursor.GetString (phonesCursor.GetColumnIndex (ContactsContract.CommonDataKinds.Phone.));
//
//
//				var ik = 10;
//				res.Add (
//					new LocalContactPhone {
//						Number = pno,
//						Type = LocalContactPhoneType.Mobile
//					});
//			}
//		}
	}
}

