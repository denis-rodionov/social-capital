using System;
using SocialCapital.Data.Model;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using SocialCapital.Data;
using SocialCapital.Data.Model.Enums;
using SocialCapital.Data.Managers;
using Ninject;

namespace SocialCapital.ViewModels.Commands
{
	public class EmailWriteCommand : BaseContactCommand<Email>
	{
		public EmailWriteCommand (Contact contact, Func<IEnumerable<Email>> getEmails) 
			: base(contact, getEmails, AppResources.InviteToChooseEmail)
		{
		}

		protected override async Task<bool> CommandAction(Page page)
		{
			var email = await GetValue (page);
			return false;
		}

		protected override void SaveCommunication ()
		{
			var db = App.Container.Get<ContactManager> ();

			db.SaveNewCommunication (new CommunicationHistory () {
				ContactId = Contact.Id,
				Time = DateTime.Now,
				Type = CommunicationType.EmailSent
			});
		}
	}
}

