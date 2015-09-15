using System;
using SocialCapital.Data.Model.Converters;
using System.Linq;
using System.Collections.Generic;

namespace SocialCapital.Data.Synchronization
{
	

	public class Syncker
	{

		/// <summary>
		/// Import or update the contact from the given source
		/// </summary>
		/// <param name="contactConverter">The contact </param>
		public void SyncContact(BaseContactConverter contactConverter)
		{
			var dbContact = new ContactManager().GetContacts (contactConverter.Exists()).SingleOrDefault ();

			if (dbContact == null)
				SaveNewContact (contactConverter);
			else {
				var fieldsToUpdate = GetFieldsToUpdate (dbContact.Id);
				UpdateFields (fieldsToUpdate);
			}


			(resGroup.Contacts as List<Contact>).Add (savedContact);

			SaveNewContact ();
		}

		#region Implementation

		/// <summary>
		/// Updates specified fields of the contact
		/// </summary>
		/// <param name="fieldsToUpdate">Fields to update.</param>
		void UpdateFields(IEnumerable<FieldValue> fieldsToUpdate)
		{
		}

		IEnumerable<FieldValue> GetFieldsToUpdate(int contactId, SyncSource source)
		{
			var modifications = new ContactManager ().GetContactModifications (contactId);

			var res = new List<FieldValue> ();
			foreach (var m in modifications)
				if (m.Source == source)
					break;
				else
					res.AddRange (m.GetModifiedFields ());
		}

		void SaveNewContact()
		{
			var savedContact = db.SaveOrUpdateContact (bookContact, updateTime, dbContact);
		}

		#endregion
	}
}

