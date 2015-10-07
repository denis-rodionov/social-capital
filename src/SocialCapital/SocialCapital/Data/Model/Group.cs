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

		private Frequency frequency = null;
		[Ignore]
		public Frequency Frequency {
			get {
				frequency = App.Container.Get<FrequencyManager> ().GetFrequency (FrequencyId);
				return frequency;
			}
			set {
				frequency = value;
			}
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
			return string.Format ("[Group: Id={0}, Name={1}, Description={2}, FrequencyId={4}, AssignedContacts={5}, Frequency={6}]", Id, Name, Description, FrequencyId, AssignedContacts, Frequency);
		}
	}
}

