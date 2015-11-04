using System;
using SQLite.Net.Attributes;
using Xamarin.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using SocialCapital.Services.AddressBookImport;
using Ninject;
using SocialCapital.Data.Managers;
using SocialCapital.Data.Model.Enums;

namespace SocialCapital.Data.Model
{
	public class Contact : IHaveId, IEquatable<Contact>
	{
		#region DB properties

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string DisplayName { get; set; }

		public string WorkPlace { get ; set; }

		/// <summary>
		/// Small photo to display int the list or in previews
		/// </summary>
		/// <value>The photo.</value>
		public byte[] Thumbnail { get; set; }

		/// <summary>
		/// Unique Id in device address book
		/// </summary>
		public string AddressBookId { get; set; }

		/// <summary>
		/// Null - no group assigned.
		/// 0 - incorrect value
		/// </summary>
		/// <value>The group identifier.</value>
		public int? GroupId { get; set; }

		/// <summary>
		/// If not null - contact is deleted: non show in contact list.
		/// But still it is stored in db
		/// </summary>
		/// <value>The delete time.</value>
		public DateTime? DeleteTime { get; set; }

		/// <summary>
		/// Means that you see the person often so you do not need to log meeting him or her
		/// </summary>
		public bool Frequent { get; set; }

		/// <summary>
		/// Birthdate of the person. Null if unknown date
		/// </summary>
		public DateTime? Birthdate { get; set; }

		/// <summary>
		/// Time of the last update from device address book (or create)
		/// </summary>
		//public DateTime AddressBookUpdateTime { get; set; }

		/// <summary>
		/// CreateTime
		/// </summary>
		public DateTime CreateTime { get; set; } 

		#endregion

		#region Navigation Properties

		[Ignore]
		public Group Group {
			get {
				if (GroupId.HasValue)
					return  App.Container.Get<GroupsManager> ().GetGroup (GroupId.Value);
				else
					return null;
			}
		}

		[Ignore]
		public Frequency Frequency {
			get {
				if (Group != null)
					return Group.Frequency;
				else
					return null;
			}
		}

		[Ignore]
		public IEnumerable<CommunicationHistory> CommunicationHistory {
			get {
				return App.Container.Get<CommunicationManager> ().GetCommunications (c => c.ContactId == Id);
			}
		}

		[Ignore]
		public CommunicationHistory LastCommunication {
			get { return App.Container.Get<CommunicationManager> ().GetLastCommunication (Id); }
		}

		#endregion

		public override string ToString ()
		{
			return string.Format ("[Contact: Id={0}, FullName={1}, WorkPlace={2}, Photo={3}, FrequencyId={4}]", Id, DisplayName, WorkPlace, Thumbnail);
		}

		#region IEquatable implementation
		public bool Equals (Contact other)
		{
			if (other == null)
				return false;

			return DisplayName == other.DisplayName;
		}

		public override int GetHashCode ()
		{
			return DisplayName.GetHashCode ();
		}
		#endregion
	}
}

