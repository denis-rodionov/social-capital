using System;
using SQLite.Net.Attributes;
using System.Collections.Generic;
using Ninject;
using SocialCapital.Data.Model.Enums;
using SocialCapital.Data.Managers;

namespace SocialCapital.Data.Model
{
	/// <summary>
	/// Group of contcts for classifying contacts by importance status
	/// and desired frequency of meetings
	/// </summary>
	public class Group : IHaveId, IEquatable<Group>
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		/// <summary>
		/// Group name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Description which contacts to put in the group
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// If the IsArchive property is true - the properties' contacts
		/// are not tracked by application
		/// </summary>
		/// <value><c>true</c> if this instance is archive; otherwise, <c>false</c>.</value>
		public bool IsArchive { get; set; }

		/// <summary>
		/// Describes how often to contact with the person
		/// </summary>
		public int FrequencyId { get; set; }

		#region Navigation Properties

		/// <summary>
		/// AssignedContacts navigation property
		/// </summary>
		private IEnumerable<Contact> assignedContacts = null;
		[Ignore]
		public IEnumerable<Contact> AssignedContacts {
			get { 
				if (assignedContacts == null)
					assignedContacts = App.Container.Get<ContactManager> ().GetContacts (c => c.GroupId == Id);
				return assignedContacts;
			}
			set { assignedContacts = value; }
		}

		private Frequency frequency;
		[Ignore]
		public Frequency Frequency {
			get {
				if (frequency == null)
					frequency = App.Container.Get<FrequencyManager> ().GetFrequency (FrequencyId);
				return frequency;
			}
			set { frequency = value; }
		}

		#endregion

		#region IEquatable implementation
		public bool Equals (Group other)
		{
			if (other == null)
				return false;
			if (Id == 0)
				return false;

			return Id == other.Id;
		}

		public override int GetHashCode ()
		{
			return Id.GetHashCode ();
		}
		#endregion

		public override string ToString ()
		{
			return string.Format ("[Group: Id={0}, Name={1}, Description={2}, IsArchive={3}, FrequencyId={4}, AssignedContacts={5}, Frequency={6}]", Id, Name, Description, IsArchive, FrequencyId, AssignedContacts, Frequency);
		}
	}
}

