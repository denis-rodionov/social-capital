using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;

namespace SocialCapital
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

			//foreach (var c in contacts)
			//	c.Photo = ImageSource.FromFile ("generic_avatar.png");

		}
	}
}

