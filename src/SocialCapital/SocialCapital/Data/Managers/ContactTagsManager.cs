using System;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using System.Linq;
using Ninject;

namespace SocialCapital.Data.Managers
{
	public class ContactTagsManager : BaseManager<ContactTag>
	{
		public ContactTagsManager ()
		{
		}

		public IEnumerable<Tag> GetContactTags(int contactId, DataContext db = null)
		{
			if (contactId == 0)
				throw new ArgumentException ("contactId cannot be 0");

			return GetList (c => c.ContactId == contactId, db).Select (c => new Tag () { Id = c.TagId, Name = c.TagName });

//			using (var db = CreateContext()) {
//				var tags = 
//					db.Connection.Query<Tag> (
//						"select t.Id, t.Name " +
//						"from ContactTag ct " +
//						"join Tag t on ct.TagId = t.Id " +
//						"where ct.ContactId = ?", contactId);
//				return tags;
//			}
		}

		public void SaveContactTags(IEnumerable<Tag> tags, int contactId)
		{
			using (var db = CreateContext ())
			{
				var contactTags = GetContactTags (contactId, db);
				var newTags = tags.Except (contactTags).ToList ();
				var removeTags = contactTags.Except (tags).ToList ();

				var tagsToSave = newTags.Where (t => t.Id == 0).ToList ();
				App.Container.Get<TagManager> ().SaveTags (tagsToSave, db);

				AssignToContact (newTags, contactId, db);
				RemoveFromContact (removeTags, contactId, db);
			}
		}

		public void AssignToContact(IEnumerable<Tag> tags, int contactId, DataContext db)
		{
			foreach (var tag in tags) {
				if (tag.Id == 0)
					throw new ArgumentException ("Save new tags before assign to the contact");
				
				Insert (new ContactTag () { 
						ContactId = contactId, 
						TagId = tag.Id,
						TagName = tag.Name
					}, db);
			}
		}

		public void RemoveFromContact(IEnumerable<Tag> tags, int contactId, DataContext db)
		{
			foreach (var tag in tags) {
				if (tag.Id == 0)
					throw new ArgumentException ("Delete ContactTags before delete tags");

				var contactTag = Find (t => t.ContactId == contactId && t.TagId == tag.Id, db);
				if (contactTag == null)
					throw new DataManagerException (string.Format ("Cannot find contactTag with tagid={0}, contactId={1}", tag.Id, contactId));

				Delete (contactTag, db);
			}
		}

		protected override void InnerRefreshCache (DataContext db)
		{
			var contactTags = db.Connection.Table<ContactTag> ().ToList ();
			var tags = App.Container.Get<TagManager> ().GetTagList (t => true);

			foreach (var ct in contactTags)
			{
				ct.TagName = tags.SingleOrDefault (t => t.Id == ct.TagId).Name;
			}

			Cache = contactTags;
		}
	}
}

