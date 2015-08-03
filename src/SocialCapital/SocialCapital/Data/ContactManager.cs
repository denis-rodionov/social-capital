using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.IO;

namespace SocialCapital
{
	

	public class ContactManager
	{
		public ContactManager ()
		{
			//Init ();
		}

		public void Init()	
		{
			

			using (var db = new DataContext ()) {
				if (db.Connection.Table<Contact> ().Count () == 0)
				{
					Log.GetLogger ().Log ("Creating default databas...");
					var tempImage = ResourceLoader.LoadFileFromResource ("SocialCapital.Resources.generic_avatar.png");
					Log.GetLogger ().Log (string.Format ("{0} bytes readed from stream", tempImage.Length));

					db.Connection.Insert (new Contact () {
						FullName = "Иванов",
						WorkPlace = "Яндекс",
						Photo = tempImage
					});
					db.Connection.Insert (new Contact () {
						FullName = "Петров",
						WorkPlace = "Google",
						Photo = tempImage
					});
					db.Connection.Insert (new Contact () {
						FullName = "Сидоров",
						WorkPlace = "Mail.ru",
						Photo = tempImage
					});
				}
			}
		}

		public IEnumerable<Contact> Contacts { 
			get { 
				using (var db = new DataContext ()) {
					return db.Connection.Table<Contact> ();
				}
			}
		}

		public List<Contact> GetFullContactsList()
		{
			return new List<Contact> (Contacts);
		}
	}
}

