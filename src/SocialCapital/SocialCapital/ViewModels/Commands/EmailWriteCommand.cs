using System;
using SocialCapital.Data.Model;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SocialCapital.ViewModels.Commands
{
	public class EmailWriteCommand : BaseContactCommand<Email>
	{
		public EmailWriteCommand (IEnumerable<Email> emails) : base(emails, AppResources.InviteToChooseEmail)
		{
		}

		protected override Task CommandAction(Page page)
		{
			return null;
		}
	}
}

