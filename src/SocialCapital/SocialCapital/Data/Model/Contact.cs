using System;
using SQLite.Net.Attributes;
using Xamarin.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SocialCapital
{
	public class Contact
	{
		#region Database

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string FullName { get; set; }

		public string WorkPlace { get ; set; }

		public byte[] Photo { get; set; }

		#endregion

		#region AccessFields

		[Ignore]
		public ImageSource PhotoImage {
			get {
				if (Photo == null || Photo.Length == 0)
					return null;
				
				var stream = new MemoryStream(Photo);
				var res = ImageSource.FromStream(() => stream);
				return res;
			}
		}

		[Ignore]
		public IEnumerable<Tag> Tags { get; set; }

		[Ignore]
		public string TagList { get { return string.Join (",", Tags.Select(t => t.Name).ToArray ());
			} }

		#endregion
	}
}

