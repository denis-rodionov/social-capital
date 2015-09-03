using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.IO;
using SocialCapital.Data.Model;
using System.Linq;

namespace SocialCapital.Data
{
	

	public class ContactManager
	{
		public ContactManager ()
		{
			//Init ();
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

		public IEnumerable<Tag> GetContactTags(int countactId)
		{
			using (var db = new DataContext ()) {
				return
					db.Connection.Query<Tag> (
						"select t.Id, t.Name " +
						"from Tag t " +
						"join ContactTag ct on ct.TagId = t.Id " +
						"where ct.ContactId = ?", countactId);
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
			var newTags = tags.Except (contactTags);

			var tagManager = new TagManager ();
			tagManager.SaveTags (newTags);
			tagManager.AssignToContact (newTags, contactId);
		}
	}
}

