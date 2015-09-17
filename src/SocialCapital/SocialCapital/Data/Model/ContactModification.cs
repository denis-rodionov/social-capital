using System;
using SQLite.Net.Attributes;
using SocialCapital.Data.Synchronization;
using System.Collections.Generic;

namespace SocialCapital.Data.Model
{
	/// <summary>
	/// Modification record. Every time a contact created or modified -
	/// a ContactModification record must be created.
	/// </summary>
	public class ContactModification
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		/// <summary>
		/// Contact ID which modification is relate to
		/// </summary>
		public int ContactId { get; set; }

		/// <summary>
		/// Source of modification: manual from device, address book, facebook ...
		/// </summary>
		public SyncSource Source { get; set; }

		/// <summary>
		/// Bit array of modified fields 
		/// </summary>
		public long ModifiedFields { get; set; }

		/// <summary>
		/// Time of modification
		/// </summary>
		public DateTime ModifiedAt { get; set; }

		/// <summary>
		/// Is modification is the first for the contact.
		/// Is the contact created by this modification
		/// </summary>
		public bool IsFirst { get; set; }

		#region Init

		public ContactModification()
		{
		}

		public ContactModification(int contactId, SyncSource source, DateTime time, bool isFirst, IEnumerable<FieldValue> modifiedFields)
		{
			ContactId = contactId;
			Source = source;
			ModifiedAt = time;
			IsFirst = isFirst;
			SetModifiedFields (modifiedFields);
		}

		#endregion

		#region service methods

		public void SetModifiedFields(IEnumerable<FieldValue> fields)
		{
			FieldValue res = 0;
			foreach (var f in fields) {
				res |= f;
			}

			ModifiedFields = (long)res;
		}

		public IEnumerable<FieldValue> GetModifiedFields()
		{
			var res = new List<FieldValue> ();
			for (int i = 0; i < 32; i++)
				res.Add ((FieldValue)((1 << i) & ModifiedFields));

			return res;
		}

		public override string ToString ()
		{
			return string.Format ("[ContactModification: Id={0}, ContactId={1}, Source={2}, ModifiedFields={3}, ModifiedAt={4}, IsFirst={5}]", Id, ContactId, Source, ModifiedFields, ModifiedAt, IsFirst);
		}

		#endregion
	}
}

