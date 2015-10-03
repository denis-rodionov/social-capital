using System;
using SQLite.Net.Attributes;
using SocialCapital.Data.Model.Enums;

namespace SocialCapital.Data.Model
{
	/// <summary>
	/// Defines how often need to contact with the person in specified period
	/// </summary>
	public class Frequency : IHaveId, IEquatable<Frequency>
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public PeriodValues Period { get; set; }

		public int Count { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Frequency: Id={0}, Period={1}, Count={2}]", Id, Period, Count);
		}

		#region IEquatable implementation
		public bool Equals (Frequency other)
		{
			if (other == null)
				return false;

			return Period == other.Period && Count == other.Count;
		}

		public override int GetHashCode ()
		{
			return (Period.ToString () + Count.ToString ()).GetHashCode ();
		}
		#endregion
	}
}

