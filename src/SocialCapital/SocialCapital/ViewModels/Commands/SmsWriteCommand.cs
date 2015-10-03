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

namespace SocialCapital.ViewModels.Commands
{
	public class SmsWriteCommand : BaseContactCommand<Phone>
	{
		public SmsWriteCommand (Contact contact, Func<IEnumerable<Phone>> getPhones) 
			: base(contact, getPhones, AppResources.InviteToChoosePhoneNumber)
		{
		}

		protected override async Task<bool> CommandAction(Page page)
		{
			string number = await GetValue (page);

			// user canceled the operation
			if (number == string.Empty)
				return false;

			DependencyService.Get<IPhoneService> ().WriteSmS (number);
			return true;
		}

		protected override void SaveCommunication ()
		{
			var db = App.Container.Get<ContactManager> ();
			db.SaveNewCommunication (new CommunicationHistory () {
				ContactId = Contact.Id,
				Time = DateTime.Now,
				Type = CommunicationType.SmsSend
			});
		}
	}
}

