using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;

namespace SocialCapital.Views
{
	public partial class ContactsProcessingPage : ContentPage
	{
		public ContactsProcessingPage (ContactsProcessingVM vm)
		{
			InitializeComponent ();
			BindingContext = vm;
		}

		private void OnClicked(object sender, EventArgs args)
		{
			var vm = new ContactGroupListVM ();
			var page = new GroupsPage (vm);
			Navigation.PushAsync (page);
		}
	}
}

