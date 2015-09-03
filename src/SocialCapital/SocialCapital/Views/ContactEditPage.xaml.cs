using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;

namespace SocialCapital.Views
{
	public partial class ContactEditPage : ContentPage
	{
		public ContactEditPage ()
		{
			InitializeComponent ();
		}

		private void OnSubmitButtonClicked(object sender, EventArgs args)
		{
			var contact = (ContactVM)BindingContext;
			contact.Save ();

			Navigation.PopAsync ();
		}
	}
}

