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

			this.BindingContext = new ContactListViewModel ();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing ();
		}

		public void OnItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			var contact = e.SelectedItem as Contact;

			var contactEditPage = new ContactEditPage () {
				BindingContext = new ContactVM(contact)
			};

			Navigation.PushAsync(contactEditPage);
		}
	}
}

