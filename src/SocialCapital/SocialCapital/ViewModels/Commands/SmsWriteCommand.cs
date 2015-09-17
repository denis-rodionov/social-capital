using System;
using System.Collections.Generic;
using SocialCapital.Data.Model;

namespace SocialCapital.ViewModels.Commands
{
	public class SmsWriteCommand : BaseContactCommand
	{
		public SmsWriteCommand (IEnumerable<Phone> phones) : base(phones)
		{
		}
	}
}

