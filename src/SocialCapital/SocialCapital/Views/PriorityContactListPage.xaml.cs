using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;
using SocialCapital.Data.Model;

namespace SocialCapital.Views
{
	public partial class PriorityContactListPage : ContentPage
	{
		public PriorityContactListPage (PriorityContactListVM vm)
		{
			InitializeComponent ();
			BindingContext = vm;
		}

		private void OnItemAppearing(object sender, ItemVisibilityEventArgs args)
		{
			var vm = (PriorityContactListVM)BindingContext;
			var contact = (Contact)args.Item;

			vm.OnItemAppearing (contact);
		}
	}
}

