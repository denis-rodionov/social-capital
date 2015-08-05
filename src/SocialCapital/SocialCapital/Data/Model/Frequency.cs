using System;
using SQLite.Net.Attributes;

namespace SocialCapital
{
	/// <summary>
	/// Defines how often need to contact with the person in specified period
	/// </summary>
	public class Frequency
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public PeriodValues Period { get; set; }

		public int Count { get; set; }
	}
}

