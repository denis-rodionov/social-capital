using System;
using SocialCapital.Data.Model.Converters;
using System.Linq;
using System.Collections.Generic;
using SocialCapital.Data.Model;

namespace SocialCapital.Data.Synchronization
{
	

	public class Syncker
	{
		

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

			if (dbContact == null)
				res = SaveNewContact (contactConverter);
			else {
				contactConverter.DatabaseContactId = dbContact.Id;

				var fieldsToUpdate = GetFieldsToUpdate (contactConverter, dbContact);

				res = UpdateFields (contactConverter, fieldsToUpdate);
			}

			return res;
		}

		#region Implementation

		/// <summary>
		/// Updates specified fields of the contact
		/// </summary>
		/// <param name="fieldsToUpdate">Fields to update.</param>
		private ContactModification UpdateFields(BaseContactConverter contactConverter, IEnumerable<FieldValue> fieldsToUpdate)
		{
			if (fieldsToUpdate.Count () != 0) {
				new ContactManager ().SaveOrUpdateContactFields (contactConverter, fieldsToUpdate);

				return SaveModification (contactConverter, fieldsToUpdate, false);
			} else
				return null;
		}

		private IEnumerable<FieldValue> GetFieldsToUpdate(BaseContactConverter contactConverter, Contact databaseContact)
		{
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

		private ContactModification SaveNewContact(BaseContactConverter contactConverter)
		{
			var contactId = new ContactManager ().SaveContactInfo (contactConverter.GetContactInfo ());

			contactConverter.DatabaseContactId = contactId;

			// saving relations for the contact table
			UpdateFields (contactConverter, GetAllRelationFields());

			return SaveModification (contactConverter, GetAllFields(), true);
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
				false,
				fields);

			if (newModification.ContactId == 0)
				throw new ArgumentException ("ContactId cannot be null while creating modification");

			return new ContactManager ().SaveModification (newModification);
		}

		#endregion
	}
}

