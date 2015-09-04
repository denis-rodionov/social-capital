using System;
using SQLite.Net.Attributes;

namespace SocialCapital.Data.Model
{
	public class ContactTag
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Indexed]
		public int TagId { get; set; }

		[Indexed]
		public int ContactId { get; set; }

		public override string ToString ()
		{
			return string.Format ("[ContactTag: Id={0}, TagId={1}, ContactId={2}]", Id, TagId, ContactId);
		}
	}
}

