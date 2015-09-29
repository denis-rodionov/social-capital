using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;
using Ninject;
using System.Linq;
using SocialCapital.Data.Model;

namespace SocialCapital.Views
{
	public partial class GroupDetailsPage : ContentPage
	{
		public GroupDetailsPage (ContactGroupVM vm)
		{
			InitializeComponent ();
			BindingContext = vm;
		}

		protected void OnClicked(object sender, EventArgs args)
		{
			var contactList = App.Container.Get<ContactListVM> ();
			var vm = (ContactGroupVM)BindingContext;
			contactList.SelectContacts (vm.AssignedContacts);

			var page = new ContactPickerPage (contactList, OnDone);

			Navigation.PushModalAsync (page);
		}

		private void OnDone(IEnumerable<Contact> seletedContacts)
		{
			var vm = (ContactGroupVM)BindingContext;
			//var contacts = contactList.AllContacts.Where(c => c.Selected).Select(c => c.SourceContact);
			vm.Assign (seletedContacts);
		}

		private void OnEditDescriptionComplete(object sender, EventArgs args)
		{
			var vm = (ContactGroupVM)BindingContext;
			vm.UpdateGroup ();
		}
	}
}

