﻿using System;
using SQLite.Net.Attributes;
using SocialCapital.Data.Model.Enums;

namespace SocialCapital.Data.Model
{
	/// <summary>
	/// Defines how often need to contact with the person in specified period
	/// </summary>
	public class Frequency : IHasId
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public PeriodValues Period { get; set; }

		public int Count { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Frequency: Id={0}, Period={1}, Count={2}]", Id, Period, Count);
		}
	}
}

