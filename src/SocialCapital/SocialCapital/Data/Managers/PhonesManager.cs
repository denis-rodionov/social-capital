using System;
using System.Collections.Generic;
using SocialCapital.Data.Model;
using System.Linq;

namespace SocialCapital.Data.Managers
{
	public class PhonesManager : BaseManager<Phone>
	{
		public PhonesManager (Func<IDataContext> contextFactory) : base(contextFactory)
		{
		}

		public IEnumerable<Phone> GetContactPhones(int contactId)
		{
			if (contactId == 0)
				throw new ArgumentException ("contactId cannot be 0");

			return GetList (p => p.ContactId == contactId);
		}
	}
}

