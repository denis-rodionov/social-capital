using System;
using SQLite.Net.Attributes;
using SocialCapital.Data.Model.Enums;

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
	public class Email : IEquatable<Email>, ILabeled, IHaveId
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
			return string.Format ("[{0}: {1} ({2})]", Label, Address, Type);
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

		#region ILabeled implementation

		public string GetLabel ()
		{
			return Label;
		}

		public string GetValue ()
		{
			return Address;
		}

		#endregion
	}
}

