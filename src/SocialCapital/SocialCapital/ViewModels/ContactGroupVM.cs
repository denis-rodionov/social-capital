using System;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using System.Collections.Generic;

namespace SocialCapital.ViewModels
{
	public class ContactGroupVM
	{
		private Group SourceGroup;

		public ContactGroupVM (Group gr)
		{
			SourceGroup = gr;
		}

		public string Name {
			get { return SourceGroup.Name; }
			set { SourceGroup.Name = value; }
		}

		public IEnumerable<Contact> AssignedContacts {
			get { return SourceGroup.AssignedContacts; }
		}

		public IEnumerable<PeriodValues> PeriodsList { 
			get {
				foreach (var en in Enum.GetValues (typeof(PeriodValues)))
					yield return (PeriodValues)en;
			}
		}

		public Frequency Frequency { 
			get { return SourceGroup.Frequency; }
		}
	}
}

