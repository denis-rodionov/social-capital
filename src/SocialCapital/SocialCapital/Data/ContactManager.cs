﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.IO;
using SocialCapital.Data.Model;
using System.Linq;
using System.Linq.Expressions;
using SocialCapital.AddressBookImport;

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
						FullName = "Иванов",
						WorkPlace = "Яндекс",
						Photo = tempImage,
						FrequencyId = freq1
					});
					db.Connection.Insert (new ContactTag () { ContactId = ivanovId, TagId = 1 });
					db.Connection.Insert (new ContactTag () { ContactId = ivanovId, TagId = 2 });

					// contact #2
					db.Connection.Insert (new Contact () {
						FullName = "Петров",
						WorkPlace = "Google",
						Photo = tempImage
					});

					// contact #3
					db.Connection.Insert (new Contact () {
						FullName = "Сидоров",
						WorkPlace = "Mail.ru",
						Photo = tempImage
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

		public void SaveContact(AddressBookContact contact)
		{
		}

		public void UpdateContact(Contact contact, AddressBookContact bookContact)
		{
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

