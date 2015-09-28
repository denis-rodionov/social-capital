using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;
using Ninject;
using System.Linq;

namespace SocialCapital.Views
{
	public partial class GroupDetailsPage : ContentPage
	{
		public GroupDetailsPage (ContactGroupVM vm)
		{
			InitializeComponent ();
			BindingContext = vm;
		}

		protected async void OnClicked(object sender, EventArgs args)
		{
			var contactList = App.Container.Get<ContactListVM> ();
			var page = new ContactPickerPage (contactList);
			await Navigation.PushModalAsync (page);

			var vm = (ContactGroupVM)BindingContext;
			var contacts = contactList.AllContacts.Where(c => c.Selected).Select(c => c.SourceContact);
			vm.Assign (contacts);
		}
	}
}

