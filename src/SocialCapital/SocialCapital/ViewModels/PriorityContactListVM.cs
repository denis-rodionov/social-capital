using System;
using System.Collections.ObjectModel;
using Ninject;
using SocialCapital.Data.Managers;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using System.Linq;
using SocialCapital.Common;

namespace SocialCapital.ViewModels
{
	public class PriorityContactListVM
	{
		const int TakeCount = 20;

		private List<ContactVM> sourceItems = new List<ContactVM>();
		
		public ObservableCollection<ContactVM> Items { get; private set; }

		public PriorityContactListVM ()
		{
			var timing = Timing.Start ("PriorityContactListVM constructor");

			sourceItems = App.Container.Get<ContactManager> ().AllContacts
				.Select (c => new ContactVM (c))
				.Where(c => c.ContactStatus.Active)
				.OrderBy (c => c.ContactStatus.RawStatus).ToList ();
			Items = new ObservableCollection<ContactVM> ();

			LoadMore ();

			timing.Finish (LogLevel.Trace);
		}

		public void OnItemAppearing(ContactVM contact)
		{
			if (Items.Count == 0)
				return;

			if (contact == Items.Last ())
				LoadMore ();
		}

		private void LoadMore()
		{
			var timing = Timing.Start ("LoadMode");

			var loaded = Items.Count;
			var newItems = sourceItems.Skip (loaded).Take (TakeCount);

			foreach (var item in newItems)
				Items.Add (item);

			timing.Finish (LogLevel.Trace);
		}
	}
}

