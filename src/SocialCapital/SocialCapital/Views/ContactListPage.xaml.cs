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

			this.BindingContext = new ContactListVM ();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing ();
		}

		public void OnItemSelected (object sender,  EventArgs args)
		{
			var cell = (Cell)sender;
			var contact = cell.BindingContext as ContactVM;

			var detailsContactPage = new ContactDetailsPage () {
				BindingContext = contact
			};

			Navigation.PushAsync(detailsContactPage);

			// contact.Reload ();
		}
	}
}

