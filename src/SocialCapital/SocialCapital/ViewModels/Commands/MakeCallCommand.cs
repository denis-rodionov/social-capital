using System;
using System.Windows.Input;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using SocialCapital.PhoneServices;
using System.Threading.Tasks;
using SocialCapital.Data;
using SocialCapital.Data.Model.Enums;
using SocialCapital.Data.Managers;
using Ninject;
using SocialCapital.Common.FormsMVVM;

namespace SocialCapital.ViewModels.Commands
{
	public class MakeCallCommand : BaseActionCommand<Phone>
	{
		public MakeCallCommand (Contact contact, Func<IEnumerable<Phone>> getPhones, IDialogProvider dialogService) 
			: base(contact, getPhones, AppResources.InviteToChoosePhoneNumber, dialogService)
		{
		}

		protected override async Task<bool> CommandAction(string number)
		{
			return DependencyService.Get<IPhoneService> ().Call (number);
		}

		protected override void SaveCommunication()
		{
			var db = App.Container.Get<CommunicationManager>();

			db.SaveNewCommunication (new CommunicationHistory () {
				ContactId = Contact.Id,
				Time = DateTime.Now,
				Type = CommunicationType.PhoneCall
			});
		}


	}
}

