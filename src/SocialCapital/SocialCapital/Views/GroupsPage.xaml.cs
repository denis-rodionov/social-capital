using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using System.Linq;

namespace SocialCapital.Views
{
	public partial class GroupsPage : ContentPage
	{
		bool FirstAppearing { get; set; }

		public GroupsPage (ContactGroupListVM vm)
		{
			InitializeComponent ();
			BindingContext = vm;
			FirstAppearing = true;
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			if (!FirstAppearing)
				(BindingContext as ContactGroupListVM).Refresh ();
		}

		protected void OnItemTapped(object sender, ItemTappedEventArgs args)
		{
			var group = (ContactGroupVM)args.Item;
			var page = new GroupDetailsPage (group);

			FirstAppearing = false;
			Navigation.PushAsync (page);
		}
	}
}

