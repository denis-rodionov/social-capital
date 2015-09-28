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
		public GroupsPage (ContactGroupListVM vm)
		{
			InitializeComponent ();
			BindingContext = vm;

			// populating the table
//			foreach (var group in vm.UnusedGroup)
//				UnusedGroupSection.Add (CreateCell (group));
//
//			foreach (var group in vm.UsedGroups)
//				UsedGtoupsSection.Add (CreateCell (group));
		}

		static TextCell CreateCell (Group group)
		{
			var newCell = new TextCell () {
				Text = group.Name,
				Detail = string.Format ("{0}: {1}", AppResources.ContactCountInGroup, group.AssignedContacts.Count ())	
			};
			return newCell;
		}

		protected void OnItemTapped(object sender, ItemTappedEventArgs args)
		{
			var group = (ContactGroupVM)args.Item;
			var page = new GroupDetailsPage (group);

			Navigation.PushAsync (page);
		}
	}
}

