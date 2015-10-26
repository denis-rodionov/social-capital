using System;
using SocialCapital.Data.Managers;
using System.Threading.Tasks;
using System.Linq;

namespace SocialCapital.ViewModels
{
	public class SummaryVM
	{
		#region Init

		readonly ContactManager contactManager;

		public SummaryVM (ContactManager contactManager)
		{
			this.contactManager = contactManager;
			Init ();
		}

		private void Init()
		{
			NotProcessedContacts = contactManager.GetContacts (c => c.GroupId != null).Count ();
			TotalContactsCount = contactManager.AllContacts .Count ();
			ProcessedContacts = TotalContactsCount - NotProcessedContacts;
		}

		#endregion

		#region Property

		public int NotProcessedContacts { get; set; }

		public int ProcessedContacts { get; set; }

		public int TotalContactsCount { get; set; }

		#endregion
	}
}

