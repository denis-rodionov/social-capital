using System;
using SocialCapital.AddressBookImport;
using System.Collections.Generic;

namespace SocialCapital.Data.Model.Converters
{
	public class AddressBookContactConverter
	{
		public AddressBookContact BookContact { get; private set; }

		public DateTime CurrentTime { get; private set; }

		public Contact DatabaseContact { get; set; }

		/// <summary>
		/// Constructor for the converter class
		/// </summary>
		/// <param name="bookContact">Device address book contact</param>
		/// <param name="time">Time of update/create</param>
		/// <param name="modelContact">Existing contact in database</param>
		public AddressBookContactConverter (AddressBookContact bookContact, DateTime time, Contact modelContact = null)
		{
			BookContact = bookContact;
			CurrentTime = time;
			DatabaseContact = modelContact;
		}

		/// <summary>
		/// Fill the Contact-model structure
		/// </summary>
		/// <returns>The contact.</returns>
		public Contact GetContact ()
		{
			return new Contact () {
				Id = DatabaseContact == null ? 0 : DatabaseContact.Id,
				AddressBookId = BookContact.Id,
				DisplayName = BookContact.DisplayName,
				Thumbnail = BookContact.Thumbnail,
				AddressBookUpdateTime = CurrentTime,
				WorkPlace = string.Join(", ", BookContact.Organizations),
				CreateTime = DatabaseContact == null ? CurrentTime : DatabaseContact.CreateTime,
				FrequencyId = 0
			};
		}

		/// <summary>
		/// Gets the phones with specified contactId
		/// </summary>
		/// <returns>The contact phones.</returns>
		/// <param name="contactId">Contact id in database</param>
		public IEnumerable<Phone> GetContactPhones(int contactId)
		{
			foreach (var phone in BookContact.Phones)
				phone.ContactId = contactId;

			return BookContact.Phones;
		}

		/// <summary>
		/// Gets the emails with the specified contactId
		/// </summary>
		/// <returns>The contact emails.</returns>
		/// <param name="contactId">Contact id in database.</param>
		public IEnumerable<Email> GetContactEmails(int contactId)
		{
			foreach (var email in BookContact.Emails)
				email.ContactId = contactId;

			return BookContact.Emails;
		}

		/// <summary>
		/// Gets the addresses with the specified contactId
		/// </summary>
		/// <returns>The contact emails.</returns>
		/// <param name="contactId">Contact id in database.</param>
		public IEnumerable<Address> GetContactAddresses(int contactId)
		{
			foreach (var address in BookContact.Emails)
				address.ContactId = contactId;

			return BookContact.Addresses;
		}
	}
}

