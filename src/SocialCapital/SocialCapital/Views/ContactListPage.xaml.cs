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
			Title = AppResources.ContactListTitle;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing ();
		}

		public async void OnItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			var contact = e.SelectedItem as ContactVM;

			var contactEditPage = new ContactEditPage () {
				//BindingContext = contact
			};

			contactEditPage.BindingContext = contact;

			await Navigation.PushAsync(contactEditPage);

			contact.Reload ();
		}
	}
}

