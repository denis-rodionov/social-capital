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
		public ContactManager ()
		{
		}

		public void Init()	
		{
			

			using (var db = new DataContext ()) {
				if (db.Connection.Table<Contact> ().Count () == 0)
				{
					Log.GetLogger ().Log ("Creating default databas...");
					var tempImage = ResourceLoader.LoadFileFromResource ("SocialCapital.Resources.generic_avatar.png");

					// contact #1
					var freq1 = db.Connection.Insert (new Frequency () { Period = PeriodValues.Month, Count = 2 });
					//Log.GetLogger ().Log ("Inserted frequency id = " + freq1);

					var ivanovId = db.Connection.Insert (new Contact () {
						DisplayName = "Иванов",
						WorkPlace = "Яндекс",
						Thumbnail = tempImage,
						FrequencyId = freq1
					});
					db.Connection.Insert (new ContactTag () { ContactId = ivanovId, TagId = 1 });
					db.Connection.Insert (new ContactTag () { ContactId = ivanovId, TagId = 2 });

					// contact #2
					db.Connection.Insert (new Contact () {
						DisplayName = "Петров",
						WorkPlace = "Google",
						Thumbnail = tempImage
					});

					// contact #3
					db.Connection.Insert (new Contact () {
						DisplayName = "Сидоров",
						WorkPlace = "Mail.ru",
						Thumbnail = tempImage
					});
				}
			}
		}

		public IEnumerable<Contact> Contacts { 
			get { 
				using (var db = new DataContext ()) {
					return db.Connection.Table<Contact> ();					
				}
			}
		}

		public IEnumerable<Contact> GetContacts(Expression<Func<Contact, bool>> whereClause)
		{
			using (var db = new DataContext ()) {
				return db.Connection.Table<Contact> ().Where (whereClause);
			}
		}

		public Contact GetContact(int contactId)
		{
			return Contacts.Single (c => c.Id == contactId);
		}

		/*
		public List<Contact> GetContactListPreview()
		{
			var res = new List<Contact> ();

			foreach (var contact in Contacts)
				res.Add (GetContactPreview (contact));

			return res;
		}

		public Contact GetContactPreview(Contact contact)
		{
			var res = new Contact () {
				Id = contact.Id,
				FullName = contact.FullName,
				WorkPlace = contact.WorkPlace,
				Photo = contact.Photo,
				FrequencyId = contact.FrequencyId
			};

			return res;
		}
		*/

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
		public void SaveOrUpdateContact (AddressBookContact bookContact, DateTime updateTime, Contact contact = null)
		{
			var converter = new AddressBookContactConverter (bookContact, updateTime, contact);
			using (var db = new DataContext ()) {
				var contactId = db.Connection.InsertOrReplace (converter.GetContact ());
				UpdateContactList<Phone> (converter.GetContactPhones (contactId), contactId, db, p => p.ContactId == contactId);
				UpdateContactList<Email> (converter.GetContactEmails (contactId), contactId, db, p => p.ContactId == contactId);
				UpdateContactList<Address> (converter.GetContactAddresses (contactId), contactId, db, p => p.ContactId == contactId);
			}
		}


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

