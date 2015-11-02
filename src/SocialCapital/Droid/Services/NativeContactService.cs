using System;
using SocialCapital.Services.AddressBookImport;
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
using Android.Content.Res;

[assembly: Dependency(typeof(SocialCapital.Droid.Services.NativeContactService))]

namespace SocialCapital.Droid.Services
{
	public class NativeContactService : BaseAddressBookService, IAddressBookInformation
	{
		public NativeContactService ()
		{
		}

		#region interface IAddressBookInformation

		public IEnumerable<AddressBookContact> GetContacts (long lastTimeStamp = 0)
		{
			return GetAllContacts (lastTimeStamp);
		}

		#endregion

		#region Implementation

		// Populate the contact list based on account currently selected in the account spinner.
		private List<AddressBookContact> GetAllContacts (long lastTimeStamp)
		{
			var uri = ContactsContract.Contacts.ContentUri;

			string[] projection = GetProjections ();

			var timing = Timing.Start ("Get contacts list");

			var context = Forms.Context.ApplicationContext;
			var content = context.ContentResolver;
			var cursor = content.Query (uri, projection, null, null, null);

			var contactList = new List<AddressBookContact> ();  
			if (cursor.MoveToFirst ()) {
				do {
					
					var newContact = new AddressBookContact() {
						Id = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.Id)),
						DisplayName = cursor.GetString (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.DisplayName)),
						LastUpdatedTimespamp = cursor.GetLong (cursor.GetColumnIndex (ContactsContract.Contacts.InterfaceConsts.ContactLastUpdatedTimestamp)),
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

					if (lastTimeStamp != 0)
					{
						if (newContact.LastUpdatedTimespamp > lastTimeStamp)
						{
							LoadExtras(context, null, newContact);
							contactList.Add(newContact);
						}
					}
					else
						contactList.Add (newContact);

				} while (cursor.MoveToNext());
			}

			if (lastTimeStamp == 0)		// load all contacts
				LoadExtras (context, contactList);

			DeletePhoneDuplicates (contactList);

			var res = PostProcess (contactList);
			RaiseCountCalculated (res.Count ());

			timing.Finish ();

			return res;
		}

		private bool FilterServicePhones(string phoneNumber)
		{
			return !phoneNumber.Contains ("*");
		}

		private string[] GetProjections()
		{
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

			return projection;
		}

		private List<AddressBookContact> PostProcess(IEnumerable<AddressBookContact> contacts)
		{
			var res = new List<AddressBookContact> ();

			foreach (var contact in contacts)
				if (contact.HasPhoneNumber == "1")
					res.Add (ProcessContact (contact));

			return res;
		}

		private AddressBookContact ProcessContact(AddressBookContact contact)
		{
			contact.Id = contact.LookupKey;
			return contact;
		}

		/// <summary>
		/// Because contacts somehow have duplicates phone we need to clear them
		/// </summary>
		private void DeletePhoneDuplicates(IEnumerable<AddressBookContact> contacts)
		{
			foreach (var contact in contacts)
			{
				if (contact.Phones.Count > 1)
				{
					var dupPhones = contact.Phones.GroupBy (x => x.NormalNumber ())
						.Where (g => g.Count () > 1)
						.ToDictionary (x => x.Key, y => y.ToList ());

					if (dupPhones.Any ())
						foreach (var dup in dupPhones.Keys)
						{
							var list = dupPhones [dup];

							if (list.Count < 2)
								throw new Exception ("Algorithm error");

							if (list [0].Number.Length > list [1].Number.Length)
								contact.Phones.Remove (list [1]);
							else
								contact.Phones.Remove (list [0]);
						}
				}
			}
		}

		private ICursor GetCursor(Android.Net.Uri uri, ContentResolver content, IEnumerable<AddressBookContact> contacts = null, AddressBookContact forContact = null)
		{
			if ((contacts != null && forContact != null) || (contacts == null && forContact == null))
				throw new ArgumentException ("Only one of values must be set (contacts or filterContactId)");

			return content.Query(
				uri,
				null,
				forContact == null ? null : "contact_id=" + forContact.Id,
				null,
				null);
		}

		private void BindToContact<T>(string contactId, T item, IEnumerable<AddressBookContact> contacts, string propertyName)
		{
			var contact = contacts.SingleOrDefault (c => c.Id == contactId);

			if (contact != null)
				BindToProperty (contact, propertyName, item);
			else
				Log.GetLogger ().Log (string.Format ("Cannot find contact with id = {0}", contactId));
		}

		private void BindToProperty<T>(AddressBookContact contact, string propertyName, T item)
		{
			var prop = contact.GetType ().GetProperty (propertyName);
			var val = prop.GetValue (contact);
			if (val != null && val.GetType().Name.Contains("List"))	// then it is a list
				(val as List<T>).Add (item);
			else
				prop.SetValue (contact, item);
		}

		private void Bind<T>(string propertyName, T item, IEnumerable<AddressBookContact> contacts, string androidContactId, AddressBookContact forContact)
		{
			if (contacts != null)
				BindToContact (androidContactId, item, contacts, propertyName);
			else
			{
				BindToProperty (forContact, propertyName, item);
			}
		}

		private void LoadExtras(Context context, IEnumerable<AddressBookContact> contacts = null, AddressBookContact forContact = null)
		{
			var content = context.ContentResolver;
			var resources = context.Resources;
			var cursor = GetCursor (ContactsContract.Data.ContentUri, content, contacts, forContact);
			var res = new List<Organization> ();

			while (cursor.MoveToNext ()) 
			{
				string dataType = cursor.GetString (cursor.GetColumnIndex (ContactsContract.DataColumns.Mimetype));
				string contactId = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Organization.InterfaceConsts.ContactId));

				switch (dataType)
				{
					case ContactsContract.CommonDataKinds.Organization.ContentItemType:
						var org = GetOrganization (cursor, resources);
						Bind ("Organizations", org, contacts, contactId, forContact);
						break;
					case ContactsContract.CommonDataKinds.StructuredPostal.ContentItemType:
						var address = GetAddress (cursor, resources);
						Bind ("Addresses", address, contacts, contactId, forContact);
						break;	
					case ContactsContract.CommonDataKinds.Note.ContentItemType:
						var note = GetNote (cursor);
						if (!string.IsNullOrEmpty (note.Contents))
							Bind ("Notes", note, contacts, contactId, forContact);
						break;
					case ContactsContract.CommonDataKinds.Photo.ContentItemType:
						byte[] photo = cursor.GetBlob (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Photo.PhotoColumnId));
						if (photo != null)
							Bind ("Thumbnail", photo, contacts, contactId, forContact);
						break;
					case ContactsContract.CommonDataKinds.Phone.ContentItemType:
						var phone = GetPhone (cursor, resources);
						if (FilterServicePhones(phone))
							Bind ("Phones", phone, contacts, contactId, forContact);
						break;
					case ContactsContract.CommonDataKinds.Email.ContentItemType:
						var email = GetEmail (cursor, resources);
						Bind ("Emails", email, contacts, contactId, forContact);
						break;
				}
			}

			cursor.Close ();
		}

		#endregion

		#region Load Emails

		private Email GetEmail(ICursor cursor, Resources resources)
		{
			var email = new Email ();
			email.Address = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Email.Address));

			EmailDataKind pkind = (EmailDataKind) cursor.GetInt (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Type));
			email.Type = ToEmailType(pkind);

			if (pkind != EmailDataKind.Custom)
				email.Label = ContactsContract.CommonDataKinds.Email.GetTypeLabel (resources, pkind, String.Empty);
			else
				email.Label = cursor.GetString (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Label));

			return email;
		}

		private List<Email> LoadEmails(Context context, IEnumerable<AddressBookContact> contacts = null, AddressBookContact forContact = null)
		{
			var cursor = GetCursor (ContactsContract.CommonDataKinds.Email.ContentUri, context.ContentResolver, contacts, forContact);
			var res = new List<Email> ();

			while (cursor.MoveToNext ()) 
			{
				string contactId = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId));
				var email = GetEmail (cursor, context.Resources);
				Bind ("Emails", email, contacts, contactId, forContact);
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

		private Phone GetPhone(ICursor cursor, Resources resources)
		{
			var phone = new Phone ();
			phone.Number = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Phone.Number));

			PhoneDataKind pkind = (PhoneDataKind) cursor.GetInt (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Type));
			phone.Type = ToPhoneType(pkind);

			if (pkind != PhoneDataKind.Custom)
				phone.Label = ContactsContract.CommonDataKinds.Phone.GetTypeLabel (resources, pkind, String.Empty);
			else
				phone.Label = cursor.GetString (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Label));

			return phone;
		}

		private List<Phone> LoadPhones(Context context, IEnumerable<AddressBookContact> contacts = null, AddressBookContact forContact = null)
		{
			var cursor = GetCursor (ContactsContract.CommonDataKinds.Phone.ContentUri, context.ContentResolver, contacts, forContact);
			var res = new List<Phone> ();

			while (cursor.MoveToNext ()) 
			{
				string contactId = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId));				
				var phone = GetPhone (cursor, context.Resources);
				Bind ("Phones", phone, contacts, contactId, forContact);
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

		private Organization GetOrganization(ICursor cursor, Resources resources)
		{
			var company = new Organization ();
			company.Name = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Organization.Company));
			company.ContactTitle = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.Organization.Title));

			OrganizationDataKind pkind = (OrganizationDataKind) cursor.GetInt (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Type));

			if (pkind != OrganizationDataKind.Custom)
				company.Label = ContactsContract.CommonDataKinds.Organization.GetTypeLabel (resources, pkind, String.Empty);
			else
				company.Label = cursor.GetString (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Label));

			return company;
		}

		#endregion

		#region Load Addresses

		private Address GetAddress(ICursor cursor, Resources resources)
		{
			var address = new Address ();
			address.Country = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.Country));
			address.Region = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.Region));
			address.City = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.City));
			address.PostalCode = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.Postcode));
			address.StreetAddress = cursor.GetString (cursor.GetColumnIndex (ContactsContract.CommonDataKinds.StructuredPostal.Street));

			var pkind = (AddressDataKind) cursor.GetInt (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Type));

			if (pkind != AddressDataKind.Custom)
				address.Label = ContactsContract.CommonDataKinds.StructuredPostal.GetTypeLabel (resources, pkind, String.Empty);
			else
				address.Label = cursor.GetString (cursor.GetColumnIndex (Android.Provider.ContactsContract.CommonDataKinds.CommonColumns.Label));

			return address;
		}

		#endregion

		#region Load Notes

		private Note GetNote(ICursor cursor)
		{
			var note = new Note ();
			note.Contents = cursor.GetString (cursor.GetColumnIndex (ContactsContract.DataColumns.Data1));
			return note;
		}

		#endregion


	}
}

