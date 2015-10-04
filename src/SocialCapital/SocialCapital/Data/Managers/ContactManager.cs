﻿using System;
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
using SocialCapital.Data.Managers;

namespace SocialCapital.Data.Managers
{
	public class ContactManager : BaseManager<Contact>, IDisposable
	{
		#region Cache

		private static IEnumerable<Contact> ContactsCache = null;

		private static IEnumerable<Email> EmailCache = null;

		#endregion

		public PhonesManager PhoneDB { get; set; }

		public EmailManager EmailDB { get; set; }

		#region Init

		public ContactManager ()
		{
			PhoneDB = new PhonesManager ();
			EmailDB = new EmailManager ();
		}

		public void Init()	
		{
		}

		#endregion

		#region Contact lists

		public IEnumerable<Contact> Contacts { 
			get { 
				return GetContacts (c => true);
			}
		}

		public IEnumerable<Contact> GetContacts(Func<Contact, bool> whereClause)
		{
			using (var db = new DataContext ()) {
				return GetList (whereClause, db);
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
			using (var db = new DataContext ())
			{
				return Get (contactId, db);
			}
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
							PhoneDB.UpdateList (contactConverter.GetPhones (), 
								db, 
								p => p.ContactId == contactConverter.DatabaseContactId);							
							break;
						case FieldValue.Emails:
							EmailDB.UpdateList (contactConverter.GetEmails (), 
								db, 
								p => p.ContactId == contactConverter.DatabaseContactId);
							break;
					case FieldValue.Address:
							SaveOrUpdateAddress (contactConverter.GetAddress (), contactConverter.DatabaseContactId, db);
							break;
						default: 
						throw new DataManagerException (string.Format ("Unknown field '{0}' to update", field));
					}
				}
				if (contactInfoFields.Any())
					UpdateContactInfoFields (contactConverter, contactInfoFields, db);
			}
		}

		public ContactModification SaveModification(ContactModification modification)
		{
			using (var db = new DataContext ()) {
				db.Connection.Insert (modification);
				if (modification.Id == 0)
					throw new DataManagerException ("Inserterd modification cannot has Id=0");

				return modification;
			}
		}

		public int SaveNewCommunication(CommunicationHistory communication)
		{
			using (var db = new DataContext ())
			{
				db.Connection.Insert (communication);

				if (communication.Id == 0)
					throw new DataManagerException ("Cannot save communication: Id = 0");

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

				RefreshCache (db);
			}
		}

		#endregion

		#region Implementation

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
				Insert (contact, db);
			}
			else
				Update (contact, db);

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
				throw new DataManagerException ("No contact exists in database with such id");

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
						throw new DataManagerException ("Unknown field for the table 'Contact'");
				}
			}

			SaveContactInfo (dbContact, db);
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
		}

		#endregion
	}
}
