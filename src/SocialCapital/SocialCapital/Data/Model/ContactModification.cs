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

		public int ContactId { get; set; }

		public SyncSource Source { get; set; }

		public long ModifiedFields { get; set; }

		public DateTime ModifiedAt { get; set; }

		#region service methods

		public IEnumerable<FieldValue> GetModifiedFields()
		{
			var res = List<FieldValue> ();

			var bitField = (FieldValue)ModifiedFields;
			foreach (var flag in Enum.GetValues(FieldValue)) {
				if (bitField.HasFlag (flag))
					res.Add (flag);
			}

			return res;
		}

		#endregion
	}
}

