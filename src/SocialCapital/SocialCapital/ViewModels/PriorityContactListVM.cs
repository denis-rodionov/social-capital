using System;
using System.Collections.ObjectModel;
using Ninject;
using SocialCapital.Data.Managers;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace SocialCapital.ViewModels
{
	public class PriorityContactListVM
	{
		const int TakeCount = 20;

		private List<Contact> sourceItems = new List<Contact>();
		
		public ObservableCollection<ContactVM> Items { get; private set; }

		public PriorityContactListVM ()
		{
			sourceItems = App.Container.Get<ContactManager> ().GetContacts (c => true).ToList();
			Items = new ObservableCollection<ContactVM> ();

			LoadMore ();
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
			var loaded = Items.Count;
			var newItems = sourceItems.Skip (loaded).Take (TakeCount).Select (c => new ContactVM (c));

			foreach (var item in newItems)
				Items.Add (item);
		}
	}
}

