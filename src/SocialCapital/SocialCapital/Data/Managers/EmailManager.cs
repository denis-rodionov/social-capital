using System;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace SocialCapital.Data.Managers
{
	public class EmailManager : BaseManager<Email>
	{
		public EmailManager ()
		{
		}

		public IEnumerable<Email> GetContactEmails(int contactId)
		{
			if (contactId == 0)
				throw new ArgumentException ("ContactId cannot be 0");

			using (var db = new DataContext ())
			{
				return GetList (c => c.ContactId == contactId, db);
			}
		}
	}
}

