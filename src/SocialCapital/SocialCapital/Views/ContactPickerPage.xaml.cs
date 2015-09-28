using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SocialCapital.ViewModels;

namespace SocialCapital.Views
{
	public partial class ContactPickerPage : ContentPage
	{
		public ContactPickerPage (ContactListVM vm)
		{
			InitializeComponent ();
			BindingContext = vm;
		}

		protected void OnDoneClicked(object sender, EventArgs args)
		{
			Navigation.PopAsync ();
		}
	}
}

