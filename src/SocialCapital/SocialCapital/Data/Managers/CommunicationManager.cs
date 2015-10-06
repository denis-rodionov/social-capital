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
			using (var db = new DataContext ())
			{
				return GetList (filter, db);
			}
		}

		public CommunicationHistory GetLastCommunication(int contactId)
		{
			using (var db = new DataContext ())
			{
				var list = GetList (c => c.ContactId == contactId, db);

				var last = list.OrderByDescending (key => key.Time).FirstOrDefault ();

				return last;
			}
		}
	}
}

