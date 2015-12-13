using System;
using Xamarin.Forms;
using SocialCapital.Data.Model;

namespace SocialCapital.ViewModels
{
	public class ContactDetailsVM : ContactVM
	{
		public ContactDetailsVM (Contact contact) : base (contact)
		{
		}

		public ContactDetailsVM (ContactVM contact) : base (contact)
		{
		}

		public ImageSource ContactPhoto {
			get	{
				var fromDb = PhotoImage;

				if (fromDb == null)
					return ImageSource.FromFile ("avatar_placeholder.gif");
				else
					return fromDb;
			}
		}
	}
}

