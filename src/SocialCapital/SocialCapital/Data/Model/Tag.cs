using System;
using SQLite.Net.Attributes;

namespace SocialCapital.Data.Model
{
	public class Tag
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string Name { get; set; }
	}
}

