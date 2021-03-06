﻿using System;
using SocialCapital.Data.Model;
using System.Linq;
using System.Threading.Tasks;
using SocialCapital.Data.Managers;
using Ninject;

namespace SocialCapital.ViewModels
{
	public class ModificationVM : ViewModelBase
	{
		readonly ContactManager contactManager;

		public ModificationVM (ContactModification modification)
		{			
			this.contactManager = App.Container.Get<ContactManager> ();

			var contact = contactManager.GetContact (modification.ContactId);
			ContactName = contact.DisplayName;
			Modification = modification;
		}

		public ContactModification Modification { get; private set; }

		string contactName = "";
		public string ContactName { 
			get { return contactName; }
			set { SetProperty (ref contactName, value); }
		}

		public string Status {
			get {
				if (Modification.IsFirst)
					return AppResources.PhoneContactImportedStatus;
				else
					return GetUpdateStatus ();
			}
		}

		private string GetUpdateStatus()
		{
			var mods = Modification.GetModifiedFields ();
			return string.Format ("{0}: {1}", AppResources.PhoneContactUpdateStatus, 
				mods.Count()  < 4 ? string.Join (", ", mods) : mods.Count ().ToString());
		}

	}
}

