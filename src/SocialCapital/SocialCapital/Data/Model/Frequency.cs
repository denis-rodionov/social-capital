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

		/// <summary>
		/// Name of the frequency
		/// </summary>
		[Unique]
		public string Name { get; set; }

		/// <summary>
		/// Wanted interval of communications:
		/// 0 - undefined (in case of never)
		/// 1 - day
		/// 30 - month
		/// 365 - year
		/// </summary>
		public double Period { get; set; }

		public bool Never { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Frequency: Id={0}, Name={1}, Period={2}]", Id, Name, Period);
		}

		#region IEquatable implementation
		public bool Equals (Frequency other)
		{
			if (other == null)
				return false;

			return Name == other.Name;
		}

		public override int GetHashCode ()
		{
			return Name.GetHashCode ();
		}
		#endregion
	}
}

