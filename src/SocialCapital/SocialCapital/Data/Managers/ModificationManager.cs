using System;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace SocialCapital.Data.Managers
{
	public class ModificationManager : BaseManager<ContactModification>
	{
		public IEnumerable<ContactModification> GetContactModifications(int contactId)
		{			
			if (contactId == 0)
				throw new ArgumentException ("contactId cannot be 0");

			return GetList (c => c.ContactId == contactId)
				.OrderByDescending(m => m.ModifiedAt)
				.ToList ();
		}

		public IEnumerable<ContactModification> GetContactModifications(Func<ContactModification, bool> filter)
		{
			return GetList (filter);
		}

		public ContactModification SaveModification(ContactModification modification)
		{
			return Insert (modification);
		}
	}
}

