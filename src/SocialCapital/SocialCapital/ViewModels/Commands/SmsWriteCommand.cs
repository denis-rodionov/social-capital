using System;
using System.Collections.Generic;
using SocialCapital.Data.Model;
using System.Threading.Tasks;
using Xamarin.Forms;
using SocialCapital.PhoneServices;

namespace SocialCapital.ViewModels.Commands
{
	public class SmsWriteCommand : BaseContactCommand<Phone>
	{
		public SmsWriteCommand (Func<IEnumerable<Phone>> phonesProvider) : base(phonesProvider, AppResources.InviteToChoosePhoneNumber)
		{
		}

		protected override async Task CommandAction(Page page)
		{
			string number = await GetValue (page);

			// user canceled the operation
			if (number == string.Empty)
				return;

			DependencyService.Get<IPhoneService> ().WriteSmS (number);
		}
	}
}

