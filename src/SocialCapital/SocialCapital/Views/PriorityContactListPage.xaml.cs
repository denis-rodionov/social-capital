using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;
using SocialCapital.Data.Model;
using SocialCapital.Logging;
using SocialCapital.Common;
using System.Threading.Tasks;

namespace SocialCapital.Views
{
	public partial class PriorityContactListPage : ContentPage
	{
		public PriorityContactListPage (PriorityContactListVM vm)
		{
			var timing = Timing.Start ("Priority list init");

			InitializeComponent ();

			Task.Run (() => BindingContext = vm);

			timing.Finish (LogLevel.Debug);
		}

		private void OnItemAppearing(object sender, ItemVisibilityEventArgs args)
		{
			var vm = (PriorityContactListVM)BindingContext;
			var contact = (ContactVM)args.Item;

			vm.OnItemAppearing (contact);
		}

		private void OnItemTapped(object sender, EventArgs args)
		{
			var contact = (sender as Cell).BindingContext;
			var page = new ContactDetailsPage((ContactVM)contact);
			Navigation.PushAsync(page);
		}

		public void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			if (args.SelectedItem == null) return; // don't do anything if we just de-selected the row
			// do something with e.SelectedItem
			((ListView)sender).SelectedItem = null; // de-select the row
		}
	}
}

