using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SocialCapital.Views
{
	public partial class ContactDetailsPage : ContentPage
	{
		public ContactDetailsPage ()
		{
			InitializeComponent ();
		}

		public void OnEditMenu(object sender, EventArgs EventArgs)
		{
			var editPage = new ContactEditPage () {
				BindingContext = this.BindingContext
			};

			Navigation.PushAsync (new NavigationPage(editPage));
		}
	}


}

