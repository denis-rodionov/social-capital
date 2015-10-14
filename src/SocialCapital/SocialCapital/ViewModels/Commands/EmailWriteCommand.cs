using System;
using SocialCapital.Data.Model;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using SocialCapital.Data;
using SocialCapital.Data.Model.Enums;
using SocialCapital.Data.Managers;
using Ninject;
using SocialCapital.PhoneServices;
using SocialCapital.Common.FormsMVVM;

namespace SocialCapital.ViewModels.Commands
{
	public class EmailWriteCommand : BaseActionCommand<Email>
	{
		public EmailWriteCommand (Contact contact, Func<IEnumerable<Email>> getEmails, IDialogProvider dialogService) 
			: base(contact, getEmails, AppResources.InviteToChooseEmail, dialogService)
		{
		}

		protected override async Task<bool> CommandAction(string number)
		{
			// number is email address
			return DependencyService.Get<IPhoneService> ().SendEmail (number);
		}

		protected override void SaveCommunication ()
		{
			var db = App.Container.Get<CommunicationManager> ();

			db.SaveNewCommunication (new CommunicationHistory () {
				ContactId = Contact.Id,
				Time = DateTime.Now,
				Type = CommunicationType.EmailSent
			});
		}
	}
}

