using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;

namespace SocialCapital.Views
{
	public partial class ContactEditPage : ContentPage
	{
		ContactEditVM vm;

		public ContactEditPage (ContactEditVM vm)
		{
			this.vm = vm;
			InitializeComponent ();
			BindingContext = vm;
		}

		private void OnSubmitButtonClicked(object sender, EventArgs args)
		{
			vm.Save ();

			Navigation.PopModalAsync ();
		}

		private void OnCancelButtonClicked(object sender, EventArgs args)
		{
			Navigation.PopModalAsync ();
		}

		protected void OnTagsTaped(object sender, EventArgs args)
		{
			var tags = vm.Tags;

			Navigation.PushAsync (new TagsSelectPage () { BindingContext = tags });
		}

		private void OnShowBirthdatePicker(object sender, EventArgs args)
		{
			vm.HasBirthdate = true;
		}
	}
}

