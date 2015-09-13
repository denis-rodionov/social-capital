using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.IO;
using SocialCapital.Data.Model;
using System.Linq;
using System.Linq.Expressions;
using SocialCapital.AddressBookImport;
using SocialCapital.Data.Model.Converters;

namespace SocialCapital.Data
{
	

	public class ContactManager : IDisposable
	{
		#region Contact database init

		public ContactManager ()
		{
		}

		public void Init()	
		{
//			
//
//			using (var db = new DataContext ()) {
//				if (db.Connection.Table<Contact> ().Count () == 0)
//				{
//					Log.GetLogger ().Log ("Creating default databas...");
//					var tempImage = ResourceLoader.LoadFileFromResource ("SocialCapital.Resources.generic_avatar.png");
//
//					// contact #1
//					var freq1 = db.Connection.Insert (new Frequency () { Period = PeriodValues.Month, Count = 2 });
//					Log.GetLogger ().Log ("Inserted frequency id = " + freq1);
//
//					var ivanovId = db.Connection.Insert (new Contact () {
//						DisplayName = "Иванов",
//						WorkPlace = "Яндекс",
//						Thumbnail = tempImage,
//						FrequencyId = freq1
//					});
//					db.Connection.Insert (new ContactTag () { ContactId = ivanovId, TagId = 1 });
//					db.Connection.Insert (new ContactTag () { ContactId = ivanovId, TagId = 2 });
//
//					// contact #2
//					db.Connection.Insert (new Contact () {
//						DisplayName = "Петров",
//						WorkPlace = "Google",
//						Thumbnail = tempImage
//					});
//
//					// contact #3
//					db.Connection.Insert (new Contact () {
//						DisplayName = "Сидоров",
//						WorkPlace = "Mail.ru",
//						Thumbnail = tempImage
//					});
//				}
//			}
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
				return db.Connection.Table<Contact> ().Where (whereClause).ToList ();
			}
		}

		/// <summary>
		/// Contact, grouped by specified selector and filtered
		/// </summary>
		/// <returns>The contact groups.</returns>
		/// <param name="groupingSelector">Grouping selector.</param>
		/// <param name="filter">Filtering expression</param>
		public IEnumerable<ContactGroup<T>> GetContactGroups<T>(
			Func<Contact, T> groupingSelector,
			Expression<Func<Contact, bool>> filter)
		{
			using (var db = new DataContext ()) {
				return db.Connection.Table<Contact> ()
					.Where (filter)
					.GroupBy (
					groupingSelector,
					(key, list) => new ContactGroup<T> () { GroupName = key, Contacts = list }).ToList ();;
			}
		}

		#endregion

		#region Contact Details

		public Contact GetContact(int contactId)
		{
			return Contacts.Single (c => c.Id == contactId);
		}

		public IEnumerable<Tag> GetContactTags(int countactId)
		{
			using (var db = new DataContext ()) {
				var tags = 
					db.Connection.Query<Tag> (
						"select t.Id, t.Name " +
						"from Tag t " +
						"join ContactTag ct on ct.TagId = t.Id " +
						"where ct.ContactId = ?", countactId);
				return tags;
			}
		}

		#endregion

		#region Save/Update methods

		public int SaveContactInfo(Contact contact)
		{
			using (var db = new DataContext ()) {
				if (contact.Id == 0)
					return db.Connection.Insert (contact);
				else
					return db.Connection.Update (contact);
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
		/// Saves the or update contact from the device address book
		/// </summary>
		/// <param name="bookContact">Book contact.</param>
		/// <param name="updateTime">Update time.</param>
		/// <param name="contact">If contact = null, create takes place, otherwise - update</param>
		/// <returns>Returns saved contact</returns>
		public Contact SaveOrUpdateContact (AddressBookContact bookContact, DateTime updateTime, Contact contact = null)
		{
			var converter = new AddressBookContactConverter (bookContact, updateTime, contact);
			var contactToSave = converter.GetContact ();
			int contactId;

			using (var db = new DataContext ()) {	
				if (contactToSave.Id == 0)
					contactId = db.Connection.Insert (contactToSave);
				else
					contactId = db.Connection.Update (contactToSave);

				//contactToSave.Id = contactId;
				if (contactId == 0)
					throw new Exception ("InsertOrReplace returned 0");

				UpdateContactList<Phone> (converter.GetContactPhones (contactId), contactId, db, p => p.ContactId == contactId);
				UpdateContactList<Email> (converter.GetContactEmails (contactId), contactId, db, p => p.ContactId == contactId);
				UpdateContactList<Address> (converter.GetContactAddresses (contactId), contactId, db, p => p.ContactId == contactId);
			}

			return contactToSave;
		}

		#endregion

		#region Implementation

		private void UpdateContactList<T>(IEnumerable<T> actualList, int contactId, DataContext db,
			Expression<Func<T, bool>> whereClause) where T : class
		{
			var existingList = db.Connection.Table<T> ().Where (whereClause);
			var newList = actualList.Except (existingList);
			var deleteList = existingList.Except (actualList);

			db.Connection.InsertAll (newList);

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

