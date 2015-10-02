using System;
using SQLite.Net.Attributes;

namespace SocialCapital.Data.Model
{
	public enum PhoneType {
		Home,
		HomeFax,
		Mobile,
		Other,
		Pager,
		Work,
		WorkFax
	}

	/// <summary>
	/// Database class migrated from phone Address book structure
	/// </summary>
	public class Phone : IEquatable<Phone>, ILabeled
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		/// <summary>
		/// Type of the phone: movile, home, work...
		/// </summary>
		/// <value>The label.</value>
		public string Label { get; set; }

		public string Number { get; set; }

		public int ContactId { get; set; }

		public bool IsFavorite { get; set; }

		/// <summary>
		/// Also type of the phone, but with enum
		/// </summary>
		[Ignore]
		public PhoneType Type { get; set; }

		#region IEquatable implementation
		public bool Equals (Phone other)
		{
			if (other == null)
				return false;

			return Number == other.Number && Label == other.Label;
		}

		public override int GetHashCode ()
		{
			return (Label + Number).GetHashCode ();
		}

		#endregion

		#region ILabeled implementation

		public string GetLabel ()
		{
			return Label;
		}

		public string GetValue ()
		{
			return Number;
		}

		#endregion


		public override string ToString ()
		{
			return string.Format ("{0},{1},{2}", Label, Number, Type);
		}
	}
}

