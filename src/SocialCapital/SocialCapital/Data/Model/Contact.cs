using System;
using SQLite.Net.Attributes;
using Xamarin.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using SocialCapital.AddressBookImport;
using Ninject;

namespace SocialCapital.Data.Model
{
	public class Contact
	{
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

		public int GroupId { get; set; }

		#region Navigation Properties

		private Group group = null;
		[Ignore]
		public Group Group {
			get {
				if (group != null)
				{
					if (GroupId != 0)
						group = App.Container.Get<GroupsManager> ().GetGroup (GroupId);
				}
				return group;
			}
			set { group = value; }
		}

		#endregion

		/// <summary>
		/// Time of the last update from device address book (or create)
		/// </summary>
		//public DateTime AddressBookUpdateTime { get; set; }

		/// <summary>
		/// CreateTime
		/// </summary>
		public DateTime CreateTime { get; set; } 

		#region No database properties


		#endregion

		public override string ToString ()
		{
			return string.Format ("[Contact: Id={0}, FullName={1}, WorkPlace={2}, Photo={3}, FrequencyId={4}]", Id, DisplayName, WorkPlace, Thumbnail);
		}
	}
}

