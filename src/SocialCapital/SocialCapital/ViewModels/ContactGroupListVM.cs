using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using Ninject;
using SocialCapital.Data;

namespace SocialCapital.ViewModels
{
	public class ContactGroupListVM : ViewModelBase
	{
		private IEnumerable<Contact> Contacts { get; set; }

		private List<Group> Groups { get; set; }

		public ContactGroupListVM ()
		{
			Contacts = App.Container.Get<ContactManager> ().Contacts;
			Groups = App.Container.Get<GroupsManager> ().GetAllGroups (g => true).ToList ();
		}

		#region Properties

		public IEnumerable<ListGroupVM<string, ContactGroupVM>> GroupedItems {
			get {
				var used = new ListGroupVM<string, ContactGroupVM> () {
					GroupName = AppResources.UsedGoupsSectionName,
					Elements = Groups.Where (g => g.AssignedContacts.Any ()).Select(g => new ContactGroupVM(g)).ToList()
				};
				var unused = new ListGroupVM<string, ContactGroupVM>() {
					GroupName = AppResources.UnusedGoupsSectionName,
					Elements = Groups.Where(g => !g.AssignedContacts.Any()).Select(g => new ContactGroupVM(g)).ToList()
				};

				return new List<ListGroupVM<string, ContactGroupVM>> () { used, unused };
			}
			set {
				OnPropertyChanged ();
			}
		}

		#endregion

		#region Actions

		public void Refresh()
		{
			Groups = App.Container.Get<GroupsManager> ().GetAllGroups (g => true).ToList ();
			GroupedItems = null;
		}

		public void AddGroup(Group group)
		{
			Groups.Add (group);
		}

		#endregion
	}
}

