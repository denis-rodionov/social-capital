using System;
using SQLite.Net.Attributes;

namespace SocialCapital.Data.Model
{
	public enum EmailType {
		Home,
		Other,
		Work
	}

	/// <summary>
	/// Class migrated from phone Address book structure
	/// </summary>
	public class Email : IEquatable<Email>
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string Address { get; set; }

		public string Label { get; set; }

		public int ContactId { get; set; }

		[Ignore]
		public EmailType Type { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Address={0}, Label={1}, Type={2}]", Address, Label, Type);
		}

		#region IEquatable implementation
		public bool Equals (Email other)
		{
			if (other == null)
				return false;

			return Address == other.Address && Label == other.Label;
		}

		public override int GetHashCode ()
		{
			return (Label + Address).GetHashCode ();
		}
		#endregion
	}
}

