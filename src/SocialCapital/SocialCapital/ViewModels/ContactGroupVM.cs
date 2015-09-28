using System;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using SocialCapital.Data;
using Ninject;

namespace SocialCapital.ViewModels
{
	public class ContactGroupVM : ViewModelBase
	{
		private Group SourceGroup;

		public ContactGroupVM (Group gr)
		{
			SourceGroup = gr;
		}

		#region Propertied

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

		public int FrequencyCount {
			get { return SourceGroup.Frequency.Count; }
			set { 
				SourceGroup.Frequency.Count = value;
				OnPropertyChanged ();
			}
		}

		public PeriodValues FrequencyPeriod {
			get { return SourceGroup.Frequency.Period; }
			set { 
				SourceGroup.Frequency.Period = value;
				OnPropertyChanged ();
			}
		}

		#endregion

		public void Assign(IEnumerable<Contact> contacts)
		{
			App.Container.Get<ContactManager> ().AssignToGroup (contacts, SourceGroup.Id);
		}
	}
}

