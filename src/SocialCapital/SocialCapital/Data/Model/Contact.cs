using System;
using SQLite.Net.Attributes;
using Xamarin.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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
	}
}

