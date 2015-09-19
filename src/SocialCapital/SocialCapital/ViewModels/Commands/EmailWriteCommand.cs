﻿using System;
using SocialCapital.Data.Model;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SocialCapital.ViewModels.Commands
{
	public class EmailWriteCommand : BaseContactCommand<Email>
	{
		public EmailWriteCommand (Func<IEnumerable<Email>> emailsProvider) : base(emailsProvider, AppResources.InviteToChooseEmail)
		{
		}

		protected override async Task CommandAction(Page page)
		{
			await GetValue (page);
		}
	}
}

