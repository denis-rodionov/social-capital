using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.IO;
using SocialCapital.Data.Model;
using System.Linq;
using System.Linq.Expressions;
using SocialCapital.AddressBookImport;
using SocialCapital.Data.Model.Converters;
using SocialCapital.Data.Synchronization;
using SocialCapital.Common;

namespace SocialCapital.Data
{
	

	public class ContactManager : IDisposable
	{
		#region Cache

		private static IEnumerable<Contact> ContactsCache = null;
		private static IEnumerable<Phone> PhoneCache = null;
		private static IEnumerable<Email> EmailCache = null;

		#endregion

		#region Contact database init

		public ContactManager ()
		{
		}

		public void Init()	
		{
		}

		#endregion

		#region Contact lists

		public IEnumerable<Contact> Contacts { 
			get { 
				using (var db = new DataContext ()) {
					var res = db.Connection.Table<Contact> ().ToList ();				
					return res;
				}
			}
		}

		public IEnumerable<Contact> GetContacts(Expression<Func<Contact, bool>> whereClause)
		{
			using (var db = new DataContext ()) {

				var timing1 = Timing.Start ("op1");
				var temp = db.Connection.Table<Contact> ().ToList ().Where (c => c.GroupId == 1);
				timing1.Finish ();

				var timing2 = Timing.Start ("op2");
				var temp1 = db.Connection.Table<Contact> ().Where (c => c.GroupId == 1).ToList ();
				timing2.Finish ();

				var timing3 = Timing.Start ("op3");
				var temp5 = db.Connection.Query<Contact> ("SELECT * FROM Contact WHERE GroupId=?", 1);
				timing3.Finish ();

				return db.Connection.Table<Contact> ().Where (whereClause).ToList ();
			}
		}

		#endregion

		#region Contact Details

		/// <summary>
		/// GetContact as it is
		/// </summary>
		/// <returns>The contact.</returns>
		/// <param name="contactId">Contact identifier.</param>
		public Contact GetContact(int contactId)
		{
			return Contacts.Single (c => c.Id == contactId);
		}

		public IEnumerable<Tag> GetContactTags(int contactId)
		{
			if (contactId == 0)
				throw new ArgumentException ("contactId cannot be 0");

			using (var db = new DataContext ()) {
				var tags = 
					db.Connection.Query<Tag> (
						"select t.Id, t.Name " +
						"from Tag t " +
						"join ContactTag ct on ct.TagId = t.Id " +
						"where ct.ContactId = ?", contactId);
				return tags;
			}
		}

		public IEnumerable<ContactModification> GetContactModifications(int contactId)
		{
			if (contactId == 0)
				throw new ArgumentException ("contactId cannot be 0");

			using (var db = new DataContext ()) {
				return db.Connection.Table<ContactModification> ()
					.Where (m => m.ContactId == contactId)
					.OrderByDescending(m => m.ModifiedAt)
					.ToList ();
			}
		}

		public IEnumerable<Phone> GetContactPhones(int contactId)
		{
			if (contactId == 0)
				throw new ArgumentException ("contactId cannot be 0");

			if (PhoneCache == null)
				RefreshCache (ref PhoneCache);

			return PhoneCache.Where (p => p.ContactId == contactId);
			
//			using (var db = new DataContext ()) {
//				var res = db.Connection.Table<Phone> ().Where (p => p.ContactId == contactId).ToList ();
//				return res;
//			}
		}

		public IEnumerable<Email> GetContactEmails(int contactId)
		{
			if (contactId == 0)
				throw new ArgumentException ("contactId cannot be 0");

			if (EmailCache == null)
				RefreshCache (ref EmailCache);

			return EmailCache.Where (p => p.ContactId == contactId);

//			using (var db = new DataContext ()) {
//				return db.Connection.Table<Email> ().Where (p => p.ContactId == contactId).ToList ();
//			}
		}

		public Address GetContactAddress(int contactId)
		{
			if (contactId == 0)
				throw new ArgumentException ("contactId cannot be 0");

			using (var db = new DataContext ()) {
				return db.Connection.Table<Address> ().Where (p => p.ContactId == contactId).FirstOrDefault ();
			}
		}

		public IEnumerable<ContactModification> GetContactModifications(Func<ContactModification, bool> filter)
		{
			using (var db = new DataContext ()) {
				return db.Connection.Table<ContactModification> ().Where (filter).ToList ();
			}
		}

		public IEnumerable<CommunicationHistory> GetContactCommunications(Func<CommunicationHistory, bool> filter)
		{
			using (var db = new DataContext ())
			{
				return db.Connection.Table<CommunicationHistory> ().Where (filter).ToList ();
			}
		}

		#endregion

		#region Save/Update methods

		/// <summary>
		/// Save or update contact info, stored in the Contact object (no relations)
		/// </summary>
		/// <returns>The saved or updater contact Id</returns>
		/// <param name="contact">Contact to save or update</param>
		public int SaveContactInfo(Contact contact)
		{
			using (var db = new DataContext ()) {
				return SaveContactInfo (contact, db);
			}
		}

		public void SaveContactTags(IEnumerable<Tag> tags, int contactId)
		{
			var contactTags = GetContactTags (contactId);
			var newTags = tags.Except (contactTags).ToList ();
			var removeTags = contactTags.Except (tags).ToList ();

			var tagManager = new TagManager ();
			tagManager.SaveTags (newTags);
			tagManager.AssignToContact (newTags, contactId);
			tagManager.RemoveFromContact (removeTags, contactId);
		}

		/// <summary>
		/// Updates fields and relations of the contact by given list of fields.
		/// Not specified fields are leaved untouched
		/// </summary>
		/// <param name="contactConverter">Contact converter.</param>
		/// <param name="fields">Fields to update</param>
		public void SaveOrUpdateContactFields(BaseContactConverter contactConverter, IEnumerable<FieldValue> fields)
		{
			if (contactConverter.DatabaseContactId == 0)
				throw new ArgumentException ("Converter does not have DatabaseContactId set");

			using (var db = new DataContext ()) {
				var contactInfoFields = new List<FieldValue> ();

				foreach (var field in fields) {
					switch (field) {
						case FieldValue.DisplayName:
						case FieldValue.Thumbnail:
						case FieldValue.WorkPlace:
							contactInfoFields.Add (field);
							break;
						case FieldValue.Phones:
							UpdateContactList<Phone> (contactConverter.GetPhones (), 
								db, 
								p => p.ContactId == contactConverter.DatabaseContactId);							
							break;
						case FieldValue.Emails:
							UpdateContactList<Email> (contactConverter.GetEmails (), 
								db, 
								p => p.ContactId == contactConverter.DatabaseContactId);
							break;
					case FieldValue.Address:
							SaveOrUpdateAddress (contactConverter.GetAddress (), contactConverter.DatabaseContactId, db);
							break;
						default: 
						throw new ContactManagerException (string.Format ("Unknown field '{0}' to update", field));
					}
				}
				UpdateContactInfoFields (contactConverter, contactInfoFields, db);
			}
		}

		public ContactModification SaveModification(ContactModification modification)
		{
			using (var db = new DataContext ()) {
				db.Connection.Insert (modification);
				if (modification.Id == 0)
					throw new ContactManagerException ("Inserterd modification cannot has Id=0");

				return modification;
			}
		}

		public int SaveNewCommunication(CommunicationHistory communication)
		{
			using (var db = new DataContext ())
			{
				db.Connection.Insert (communication);

				if (communication.Id == 0)
					throw new ContactManagerException ("Cannot save communication: Id = 0");

				return communication.Id;
			}
		}

		public void AssignToGroup(IEnumerable<Contact> contacts, int groupId)
		{
			var contactIds = contacts.Select (c => c.Id);

			using (var db = new DataContext ())
			{
				var assignedContacts = db.Connection.Table<Contact> ().Where (c => c.GroupId == groupId).Select (c => c.Id);
				var toAssign = contactIds.Except (assignedContacts).ToList ();
				var toUnassign = assignedContacts.Except (contactIds).ToList ();

				// assign
				foreach (var contactId in toAssign)
					db.Connection.Execute ("UPDATE Contact SET GroupId=? WHERE Id=?", groupId, contactId);

				// unassign
				foreach (var contactId in toUnassign)
					db.Connection.Execute ("UPDATE Contact SET GroupId = null WHERE Id=?", contactId);
			}
		}

		#endregion

		#region Implementation

		private void RefreshCache<T>(ref IEnumerable<T> cache) where T : class
		{
			using (var db = new DataContext ())
			{
				cache = db.Connection.Table<T> ().ToList ();
			}

			Log.GetLogger ().Log ("Cache updated for " + typeof(T));
		}

		private Address SaveOrUpdateAddress(Address item, int contactId, DataContext db) 
		{
			var dbItem = db.Connection.Table<Address> ().SingleOrDefault (c => c.ContactId == contactId);
			Address res = item;

			if (dbItem == null) {
				db.Connection.Insert (res);
			} else {
				res.Id = dbItem.Id;
				db.Connection.Update (res);
			}

			return res;
		}

		private int SaveContactInfo(Contact contact, DataContext db)
		{
			if (contact.Id == 0) {
				contact.CreateTime = DateTime.Now;
				db.Connection.Insert (contact);
			}
			else
				db.Connection.Update (contact);

			if (contact.Id == 0)
				throw new Exception (string.Format ("Contact {0} saved incorrect: Id = 0", contact.DisplayName));

			return contact.Id;
		}

		/// <summary>
		/// Updates only fields which stored in the Contact table
		/// </summary>
		/// <param name="contactConverter">Contact converter.</param>
		/// <param name="fields">Fields.</param>
		/// <param name="db">Db.</param>
		private void UpdateContactInfoFields(BaseContactConverter contactConverter, IEnumerable<FieldValue> fields, DataContext db)
		{
			var newContact = contactConverter.GetContactInfo ();
			var dbContact = GetContact (contactConverter.DatabaseContactId);

			if (dbContact == null || dbContact.Id == 0)
				throw new ContactManagerException ("No contact exists in database with such id");

			foreach (var field in fields) {
				switch (field) {
					case FieldValue.DisplayName:
						dbContact.DisplayName = newContact.DisplayName;
						break;
					case FieldValue.Thumbnail:
						dbContact.Thumbnail = newContact.Thumbnail;
						break;
					case FieldValue.WorkPlace:
						dbContact.WorkPlace = newContact.WorkPlace;
						break;
					default:
						throw new ContactManagerException ("Unknown field for the table 'Contact'");
				}
			}

			SaveContactInfo (dbContact, db);
		}

		private void UpdateContactList<T>(IEnumerable<T> actualList, DataContext db,
			Expression<Func<T, bool>> whereClause) where T : class, IEquatable<T>
		{
			var existingList = db.Connection.Table<T> ().Where (whereClause).ToList();
			var newList = actualList.Except (existingList).ToList();
			var deleteList = existingList.Except (actualList).ToList();

			foreach (var item in newList) {
				db.Connection.Insert (item);
			}

			foreach (var item in deleteList)
				db.Connection.Delete(item);
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
		}

		#endregion
	}
}

