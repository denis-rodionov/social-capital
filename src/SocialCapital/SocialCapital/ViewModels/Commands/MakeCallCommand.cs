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

namespace SocialCapital.ViewModels.Commands
{
	public class MakeCallCommand : BaseContactCommand<Phone>
	{
		public MakeCallCommand (Contact contact, Func<IEnumerable<Phone>> getPhones) 
			: base(contact, getPhones, AppResources.InviteToChoosePhoneNumber)
		{
		}

		protected override async Task<bool> CommandAction(Page page)
		{
			string number = await GetValue (page);

			// user canceled the operation
			if (number == string.Empty)
				return false;
			
			DependencyService.Get<IPhoneService> ().Call (number);
			return true;
		}

		protected override void SaveCommunication()
		{
			var db = new ContactManager ();

			db.SaveNewCommunication (new CommunicationHistory () {
				ContactId = Contact.Id,
				Time = DateTime.Now,
				Type = CommunicationType.PhoneCall
			});
		}


	}
}

