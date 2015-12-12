using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;
using Ninject;
using System.Linq;
using SocialCapital.Data.Model;
using SocialCapital.Common;

namespace SocialCapital.Views
{
	public partial class GroupDetailsPage : ContentPage
	{
		public GroupDetailsPage (ContactGroupVM vm)
		{
			var timing = Timing.Start ("GroupDetailsPage init");

			InitializeComponent ();
			BindingContext = vm;

			timing.Finish ();
		}

		/// <summary>
		/// Assign contacts to the group
		/// </summary>
		protected void OnClicked(object sender, EventArgs args)
		{
			var vm = (ContactGroupVM)BindingContext;

			var contactList = new ContactListVM (c => c.GroupId == null || c.GroupId == vm.SourceGroup.Id );

			contactList.SelectContacts (vm.AssignedContacts);

			var page = new ContactPickerPage (contactList, OnDone);

			Navigation.PushAsync (page);
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

