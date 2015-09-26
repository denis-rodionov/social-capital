using System;
using SocialCapital.Data.Model;
using System.Collections.Generic;

namespace SocialCapital.Data
{
	public class TagManager
	{
		public TagManager ()
		{
		}

		public void Init()
		{
		}

		public void SaveTags(IEnumerable<Tag> tags)
		{
			using (var db = new DataContext ()) {
				foreach (var tag in tags)
					if (tag.Id == 0)
						db.Connection.Insert (tag);
			}
		}

		public void AssignToContact(IEnumerable<Tag> tags, int contactId)
		{
			using (var db = new DataContext ()) {
				foreach (var tag in tags) {
					if (tag.Id == 0)
						throw new ArgumentException ("Save new tags before assign to the contact");

					db.Connection.Insert (new ContactTag () { 
						ContactId = contactId, 
						TagId = tag.Id });
				}
			}
		}

		public void RemoveFromContact(IEnumerable<Tag> tags, int contactId)
		{
			using (var db = new DataContext ()) {
				foreach (var tag in tags) {
					if (tag.Id == 0)
						throw new ArgumentException ("Delete ContactTags before delete tags");

					db.Connection.Execute ("DELETE FROM ContactTag WHERE TagId=? AND ContactId=?", tag.Id, contactId);
				}
			}
		}
	}
}

