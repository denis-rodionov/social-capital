using System;
using SocialCapital.Data.Model.Converters;
using System.Linq;
using System.Collections.Generic;
using SocialCapital.Data.Model;
using SocialCapital.Common;

namespace SocialCapital.Data.Synchronization
{
	

	public class Syncker
	{
		static TimeSpan update = new TimeSpan (0);
		static TimeSpan getFields = new TimeSpan (0);
		static TimeSpan save = new TimeSpan (0);

		/// <summary>
		/// Import or update the contact from the given source
		/// </summary>
		/// <param name="contactConverter">The contact </param>
		/// <returns>Returns modification in database.
		/// Returns null if no modifications made</returns>
		public ContactModification SyncContact(BaseContactConverter contactConverter)
		{
			ContactModification res;

			var dbContact = new ContactManager().GetContacts (contactConverter.IsContactExistsInDatabase()).SingleOrDefault ();

			if (dbContact == null) {
				var t2 = Timing.Start ("UpdateFields");
				SaveNewContact (contactConverter);
				update += t2.Finish (LogLevel.Trace);

				var t3 = Timing.Start ("saveModif");
				res = SaveModification (contactConverter, GetAllFields(), true);
				save += t3.Finish (LogLevel.Trace);
			}
			else {
				contactConverter.DatabaseContactId = dbContact.Id;

				var t1 = Timing.Start ("fildsToUpdate");
				var fieldsToUpdate = GetFieldsToUpdate (contactConverter, dbContact);
				getFields += t1.Finish (LogLevel.Trace);

				if (fieldsToUpdate.Any()) {
					var t2 = Timing.Start ("UpdateFields");
					UpdateFields (contactConverter, fieldsToUpdate);
					update += t2.Finish (LogLevel.Trace);

					var t3 = Timing.Start ("saveModif");
					res = SaveModification (contactConverter, fieldsToUpdate, false);
					save += t3.Finish (LogLevel.Trace);
				}
				else 
					res = null;
			}

			return res;
		}

		#region Implementation

		/// <summary>
		/// Updates specified fields of the contact
		/// </summary>
		/// <param name="fieldsToUpdate">Fields to update.</param>
		private void UpdateFields(BaseContactConverter contactConverter, IEnumerable<FieldValue> fieldsToUpdate)
		{
			if (fieldsToUpdate.Count () != 0) 
				new ContactManager ().SaveOrUpdateContactFields (contactConverter, fieldsToUpdate);
		}

		private IEnumerable<FieldValue> GetFieldsToUpdate(BaseContactConverter contactConverter, Contact databaseContact)
		{
			if (contactConverter.Source != SyncSource.AddressBook)
				throw new NotImplementedException ("GetFieldsToUpdate implemented only for addressbook");

			var changedFields = GetChangedFields (contactConverter, databaseContact);

			return changedFields;
		}

		private IEnumerable<FieldValue> GetChangedFields(BaseContactConverter contactConverter, Contact databaseContact)
		{
			var sourceContat = contactConverter.GetContactInfo ();
			var res = new List<FieldValue> ();
			var db = new ContactManager ();

			if (sourceContat.DisplayName != databaseContact.DisplayName)
				res.Add (FieldValue.DisplayName);
			if (sourceContat.Thumbnail != null && databaseContact.Thumbnail == null)
				res.Add (FieldValue.Thumbnail);
			if (sourceContat.WorkPlace != databaseContact.WorkPlace)
				res.Add (FieldValue.WorkPlace);
			if (!ListEqual (contactConverter.GetPhones (), db.GetContactPhones (contactConverter.DatabaseContactId)))
				res.Add (FieldValue.Phones);
			if (!ListEqual (contactConverter.GetEmails (), db.GetContactEmails (contactConverter.DatabaseContactId)))
				res.Add (FieldValue.Emails);

			var dbAddress = db.GetContactAddress (contactConverter.DatabaseContactId);
			var convertAddress = contactConverter.GetAddress (); 
			if (contactConverter.GetAddress () != db.GetContactAddress (contactConverter.DatabaseContactId))
				res.Add (FieldValue.Address);

			return res;
		}

		private bool ListEqual<T>(IEnumerable<T> list1, IEnumerable<T> list2)
		{
			var firstNotSecond = list1.Except (list2);
			var secondNotFirst = list2.Except (list1);

			return !firstNotSecond.Any() && !secondNotFirst.Any ();
		}

		private void SaveNewContact(BaseContactConverter contactConverter)
		{
			var contactId = new ContactManager ().SaveContactInfo (contactConverter.GetContactInfo ());

			contactConverter.DatabaseContactId = contactId;

			// saving relations for the contact table
			UpdateFields (contactConverter, GetAllRelationFields());
		}

		private IEnumerable<FieldValue> GetAllFields()
		{
			return new List<FieldValue> () { 
				FieldValue.DisplayName,
				FieldValue.Thumbnail,
				FieldValue.WorkPlace,
				FieldValue.Phones,
				FieldValue.Emails,
				FieldValue.Address
			};	
		}

		private IEnumerable<FieldValue> GetAllRelationFields()
		{
			return new List<FieldValue> () { 
				FieldValue.Phones,
				FieldValue.Emails,
				FieldValue.Address
			};
		}

		private ContactModification SaveModification(BaseContactConverter converter, IEnumerable<FieldValue> fields, bool first)
		{
			var newModification = new ContactModification (
				converter.DatabaseContactId,
				converter.Source,
				converter.SyncTime,
				first,
				fields);

			if (newModification.ContactId == 0)
				throw new ArgumentException ("ContactId cannot be null while creating modification");

			return new ContactManager ().SaveModification (newModification);
		}

		#endregion
	}
}

