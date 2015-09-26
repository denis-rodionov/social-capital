using System;
using SocialCapital.AddressBookImport;
using System.Collections.Generic;
using System.Linq;
using SocialCapital.Data.Model.Converters;
using SocialCapital.Data.Model;
using SocialCapital.Data.Synchronization;
using System.Linq.Expressions;

namespace SocialCapital.AddressBookImport
{
	public class AddressBookContactConverter : BaseContactConverter
	{
		public AddressBookContact BookContact { get; private set; }

		/// <summary>
		/// Constructor for the converter class
		/// </summary>
		/// <param name="bookContact">Device address book contact</param>
		/// <param name="time">Time of update/create</param>
		/// <param name="modelContact">Existing contact in database</param>
		public AddressBookContactConverter (AddressBookContact bookContact, DateTime time)
			: base(time, SyncSource.AddressBook)
		{
			BookContact = bookContact;
		}

		/// <summary>
		/// Fill the Contact-model structure
		/// </summary>
		/// <returns>The contact.</returns>
		public override Contact GetContactInfo ()
		{
			return new Contact () {
				Id = DatabaseContactId,
				AddressBookId = BookContact.Id,
				DisplayName = BookContact.DisplayName,
				Thumbnail = BookContact.Thumbnail,
				//AddressBookUpdateTime = SyncTime,
				WorkPlace = string.Join(", ", BookContact.Organizations.Select(o => o.Name))
			};
		}

		/// <summary>
		/// Delegate which helps to determin if the contact already copied in the device database
		/// </summary>
		public override Expression<Func<Contact, bool>> IsContactExistsInDatabase ()
		{
			return (contact) => contact.AddressBookId == BookContact.Id;
		}

		/// <summary>
		/// Gets the phones with specified contactId. DatabaseContactId must be set up
		/// </summary>
		/// <returns>The contact phone list</returns>
		public override IEnumerable<Phone> GetPhones()
		{
			if (DatabaseContactId == 0)
				throw new Exception ("Set DatabaseContactId to not 0 value, before invoke this function");

			foreach (var phone in BookContact.Phones)
				phone.ContactId = DatabaseContactId;

			return BookContact.Phones;
		}

		/// <summary>
		/// Gets the emails with the specified contactId
		/// </summary>
		/// <returns>The contact emails.</returns>
		/// <param name="contactId">Contact id in database.</param>
		public override IEnumerable<Email> GetEmails()
		{
			if (DatabaseContactId == 0)
				throw new Exception ("Set DatabaseContactId to not 0 value, before invoke this function");
			
			foreach (var email in BookContact.Emails)
				email.ContactId = DatabaseContactId;

			return BookContact.Emails;
		}

		/// <summary>
		/// Gets the address with the specified contactId
		/// </summary>
		/// <returns>The contact emails.</returns>
		/// <param name="contactId">Contact id in database.</param>
		public override Address GetAddress()
		{
			if (DatabaseContactId == 0)
				throw new Exception ("Set DatabaseContactId to not 0 value, before invoke this function");
			
			var res = BookContact.Addresses.FirstOrDefault ();

			if (res != null)
				res.ContactId = DatabaseContactId;

			return res;
		}
	}
}

