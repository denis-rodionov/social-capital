using System;
using SQLite.Net.Attributes;

namespace SocialCapital
{
	public class ContactTag
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Indexed]
		public int TagId { get; set; }

		[Indexed]
		public int ContactId { get; set; }
	}
}

