using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using System.Linq;
using SocialCapital.Common;
using Ninject;
using SocialCapital.Data;
using SocialCapital.Data.Managers;

namespace SocialCapital.Views
{
	public partial class GroupsPage : ContentPage
	{
		bool FirstAppearing { get; set; }

		public GroupsPage (ContactGroupListVM vm)
		{
			var timing = Timing.Start ("GroupsPage init");

			InitializeComponent ();
			BindingContext = vm;
			FirstAppearing = true;

			timing.Finish ();
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

		protected void OnAddGroup(object sender, EventArgs args)
		{
			var newGroup = App.Container.Get<GroupsManager> ().CreateNewGroup ();
			var vm = (ContactGroupListVM)BindingContext;

			var newVM = new ContactGroupVM (newGroup) {
				EditMode = true
			};

			FirstAppearing = false;
			Navigation.PushAsync (new GroupDetailsPage (newVM));
		}
	}
}

