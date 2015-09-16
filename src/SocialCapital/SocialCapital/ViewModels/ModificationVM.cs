using System;
using SocialCapital.Data.Model;

namespace SocialCapital.ViewModels
{
	public class ModificationVM : ViewModelBase
	{
		public ModificationVM (ContactModification modification)
		{
			var contact = Database.GetContact (modification.ContactId);
			ContactName = contact.DisplayName;
			Modification = modification;
		}

		public ContactModification Modification { get; private set; }

		public string ContactName { get; private set; }

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
			return string.Format("{0}: {1}", AppResources.PhoneContactUpdateStatus, string.Join(", ", Modification.GetModifiedFields()));
		}

	}
}

