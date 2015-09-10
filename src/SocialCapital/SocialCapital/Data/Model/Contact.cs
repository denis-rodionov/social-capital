using System;
using SQLite.Net.Attributes;
using Xamarin.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using SocialCapital.AddressBookImport;

namespace SocialCapital.Data.Model
{
	public class Contact
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string FullName { get; set; }

		public string WorkPlace { get ; set; }

		/// <summary>
		/// Small photo to display int the list or in previews
		/// </summary>
		/// <value>The photo.</value>
		public byte[] Photo { get; set; }

		/// <summary>
		/// How oftern need to contact with this person
		/// </summary>
		/// <value>The frequency identifier.</value>
		public int FrequencyId { get; set; }

		/// <summary>
		/// Unique Id in device address book
		/// </summary>
		public string AddressBookId { get; set; }

		/// <summary>
		/// Time of the last update from device address book (or create)
		/// </summary>
		public DateTime AddressBookUpdateTime { get; set; }

		/// <summary>
		/// CreateTime
		/// </summary>
		public DateTime CreateTime { get; set; } 

		//[Ignore]
		//public AddressBookContact AbContact { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Contact: Id={0}, FullName={1}, WorkPlace={2}, Photo={3}, FrequencyId={4}]", Id, FullName, WorkPlace, Photo, FrequencyId);
		}
	}
}

