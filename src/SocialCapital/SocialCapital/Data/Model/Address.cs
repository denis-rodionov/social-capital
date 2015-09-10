using System;
using SQLite.Net.Attributes;

namespace SocialCapital.Data.Model
{
	/// <summary>
	/// Class migrated from phone Address book structure
	/// </summary>
	public class Address : IEquatable<Address>
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string Country { get; set; }

		public string City { get; set; }

		public string StreetAddress { get; set; }

		public string Region { get; set; }

		public string PostalCode { get; set; }

		/// <summary>
		/// Type of the address: work or home
		/// </summary>
		public string Label { get; set; }

		public int ContactId { get; set; }

		public Address ()
		{
		}

		#region IEquatable implementation

		public bool Equals (Address other)
		{
			if (other == null)
				return false;

			return Country == other.Country &&
					City == other.City &&
					Region == other.Region &&
					StreetAddress == other.StreetAddress &&
					PostalCode == other.PostalCode &&
					Label == other.Label;
		}

		#endregion
	}
}

