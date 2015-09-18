using System;
using System.Windows.Input;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using SocialCapital.PhoneServices;
using System.Threading.Tasks;

namespace SocialCapital.ViewModels.Commands
{
	public class MakeCallCommand : BaseContactCommand<Phone>
	{
		public MakeCallCommand (IEnumerable<Phone> phones) : base(phones, AppResources.InviteToChoosePhoneNumber)
		{
		}

		protected override async Task CommandAction(Page page)
		{
			string number = await GetValue (page);

			// user canceled the operation
			if (number == string.Empty)
				return;
			
			DependencyService.Get<IPhoneService> ().Call (number);
		}


	}
}

