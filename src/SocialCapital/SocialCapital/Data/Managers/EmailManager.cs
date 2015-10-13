using System;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace SocialCapital.Data.Managers
{
	public class EmailManager : BaseManager<Email>
	{
		public EmailManager (Func<IDataContext> contextFactory) : base(contextFactory)
		{
		}

		public IEnumerable<Email> GetContactEmails(int contactId)
		{
			if (contactId == 0)
				throw new ArgumentException ("ContactId cannot be 0");

			return GetList (c => c.ContactId == contactId);
		}
	}
}

