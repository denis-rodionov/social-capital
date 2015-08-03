using System;
using SQLite.Net.Attributes;
using Xamarin.Forms;
using System.IO;

namespace SocialCapital
{
	public class Contact
	{
		#region Database

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string FullName { get; set; }

		public string WorkPlace { get ; set; }

		public byte[] Photo { get; set; }

		#endregion

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


	}
}

