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

			Navigation.PopModalAsync ();
		}

		private void OnCancelButtonClicked(object sender, EventArgs args)
		{
			var contact = (ContactVM)BindingContext;
			contact.Reload ();

			Navigation.PopModalAsync ();
		}

		protected void OnTagsTaped(object sender, EventArgs args)
		{
			var tags = (BindingContext as ContactVM).Tags;

			Navigation.PushAsync (new TagsSelectPage () { BindingContext = tags });
		}
	}
}

