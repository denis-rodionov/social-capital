using System;
using System.Collections.Generic;
using SocialCapital.Data.Model;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;

namespace SocialCapital
{
	public abstract class PhoneCommand
	{
		public IEnumerable<Phone> Phones { get; private set; }

		public PhoneCommand (IEnumerable<Phone> phones)
		{
			Phones = phones;
		}

		protected async Task<Phone> ChoosePhone(Page page)
		{
			var dict = new Dictionary<string, Phone> ();

			foreach (var phone in Phones)
				dict.Add (string.Format ("{0}\t{1}", phone.Label, phone.Number), phone);

			var label = await page.DisplayActionSheet (AppResources.InviteToChoosePhoneNumber, 
				AppResources.CancelButton, 
				null,
				dict.Keys.ToArray());

			if (label == AppResources.CancelButton)
				return null;
			else
				return dict [label];
		}
	}
}

