using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SocialCapital.ViewModels;
using SocialCapital.Data.Model;

namespace SocialCapital.Views
{
	public partial class ContactListPage : ContentPage
	{
		public ContactListPage ()
		{
			InitializeComponent ();

			this.BindingContext = new ContactListVM (c => true);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing ();
		}

		public void OnItemSelected (object sender,  EventArgs args)
		{
			var cell = (Cell)sender;
			var contact = new ContactDetailsVM (cell.BindingContext as ContactVM);
			var contactList = BindingContext as ContactListVM;

			contactListView.SelectedItem = null;

			contact.Deleted += contactList.OnDeletedContact;
		
			var detailsContactPage = new ContactDetailsPage (contact);

			Navigation.PushAsync(detailsContactPage);
		}

		public void OnListViewSelected(object sender, SelectedItemChangedEventArgs args)
		{
			if (args.SelectedItem == null) return; // don't do anything if we just de-selected the row
			// do something with e.SelectedItem
			((ListView)sender).SelectedItem = null; // de-select the row
		}

		private void OnDeleteMenuClicked(object sender, EventArgs args)
		{
		}
	}
}

