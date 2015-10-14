using System;
using System.Collections.Generic;
using SocialCapital.Data.Model;
using System.Threading.Tasks;
using Xamarin.Forms;
using SocialCapital.PhoneServices;
using SocialCapital.Data;
using SocialCapital.Data.Model.Enums;
using SocialCapital.Data.Managers;
using Ninject;
using SocialCapital.Common.FormsMVVM;

namespace SocialCapital.ViewModels.Commands
{
	public class SmsWriteCommand : BaseActionCommand<Phone>
	{
		public SmsWriteCommand (Contact contact, Func<IEnumerable<Phone>> getPhones, IDialogProvider dialogService) 
			: base(contact, getPhones, AppResources.InviteToChoosePhoneNumber, dialogService)
		{
		}

		protected override async Task<bool> CommandAction(string number)
		{
			return DependencyService.Get<IPhoneService> ().WriteSmS (number);
		}

		protected override void SaveCommunication ()
		{
			var db = App.Container.Get<CommunicationManager> ();
			db.SaveNewCommunication (new CommunicationHistory () {
				ContactId = Contact.Id,
				Time = DateTime.Now,
				Type = CommunicationType.SmsSend
			});
		}
	}
}

