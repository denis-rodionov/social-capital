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

		protected void OnTagsTaped(object sender, EventArgs args)
		{
			var tags = (BindingContext as ContactVM).Tags;

			Navigation.PushAsync (new TagsSelectPage () { BindingContext = tags });
		}

		protected void OnDisappearing(object sender, EventArgs args)
		{
			var contact = (BindingContext as ContactVM);

			contact.Reload ();
		}
	}
}

