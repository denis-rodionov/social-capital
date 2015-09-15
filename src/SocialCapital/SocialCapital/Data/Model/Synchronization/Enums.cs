using System;

namespace SocialCapital.Data.Synchronization
{
	/// <summary>
	/// Source of a contact modifications
	/// </summary>
	public enum SyncSource {
		/// <summary>
		/// Manual modified or created contact
		/// </summary>
		Manual = 1,

		/// <summary>
		/// A contact is modified or created from device address book importing
		/// </summary>
		AddressBook = 2
	}

	/// <summary>
	/// Data fields of a contact which can be imported from sources
	/// </summary>
	[Flags]
	public enum FieldValue {
		None = 0,
		DisplayName = 1,
		WorkPlace = 2,
		Thumbnail = 4,
		Phones = 8,
		Emails = 16,
		Addresses = 32
	}
}

