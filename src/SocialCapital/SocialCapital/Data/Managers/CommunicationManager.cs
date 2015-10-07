using System;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace SocialCapital.Data.Managers
{
	public class CommunicationManager : BaseManager<CommunicationHistory>
	{
		public CommunicationManager ()
		{
		}

		public IEnumerable<CommunicationHistory> GetCommunications(Func<CommunicationHistory, bool> filter)
		{
			return GetList (filter);
		}

		public CommunicationHistory GetLastCommunication(int contactId)
		{
			var list = GetList (c => c.ContactId == contactId);
			var last = list.OrderByDescending (key => key.Time).FirstOrDefault ();
			return last;
		}

		public int SaveNewCommunication(CommunicationHistory communication)
		{
			return Insert (communication).Id;
		}
	}
}

