using System;
using System.Collections.Generic;
using SocialCapital.Data.Synchronization;

namespace SocialCapital.Data.Model.Converters
{
	/// <summary>
	/// Tasks:
	/// 1. Has two states: { ContactId is known }, {ContactId is unknown }
	/// </summary>
	public abstract class BaseContactConverter
	{
		/// <summary>
		/// Time of contact import from source to database
		/// </summary>
		public DateTime SyncTime { get; set; }

		/// <summary>
		/// Gets or sets the contact identifier.
		/// </summary>
		/// <value>0 - is unset value</value>
		public int DatabaseContactId { get; set; }

		/// <summary>
		/// Source of importing
		/// </summary>
		public SyncSource Source { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="syncTime">Time of the contact current import </param>
		/// <param name="source">Source of importing</param>
		/// <param name="databaseContactId">Database contact identifier if known. Leave 0 if uncknown</param>
		public BaseContactConverter(DateTime syncTime, SyncSource source, int databaseContactId = 0)
		{
			SyncTime = syncTime;
			DatabaseContactId = databaseContactId;
			Source = source;
		}

		/// <summary>
		/// Delegate which helps to determin if the contact already copied in the device database
		/// </summary>
		public abstract Func<Contact, bool> Exists ();

		/// <summary>
		/// Returns contact information stored in the Contact class
		/// </summary>
		/// <returns>The contact info.</returns>
		public abstract Contact GetContactInfo();

		/// <summary>
		/// Returns Phones of the contact
		/// </summary>
		/// <exception cref="ConverterException">If DatabaseContactId == 0 throws the exception</exception>
		/// <returns>The contact phone list</returns>
		public abstract IEnumerable<Phone> GetPhones ();

		/// <summary>
		/// Returns emails of the contact
		/// </summary>
		/// <exception cref="ConverterException">If DatabaseContactId == 0 throws the exception</exception>
		/// <returns>The contact email list</returns>
		public abstract IEnumerable<Email> GetEmails();

		/// <summary>
		/// Returns addresses of the contact
		/// </summary>
		/// <exception cref="ConverterException">If DatabaseContactId == 0 throws the exception</exception>
		/// <returns>The contact address list</returns>
		public abstract IEnumerable<Address> GetAdresses ();
	}
}

