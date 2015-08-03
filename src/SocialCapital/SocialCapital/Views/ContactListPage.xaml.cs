using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SocialCapital
{
	public partial class ContactListPage : ContentPage
	{
		public ContactListPage ()
		{
			InitializeComponent ();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing ();

			var contacts = new ContactManager ().GetFullContactsList ();

			//foreach (var c in contacts)
			//	c.Photo = ImageSource.FromFile ("generic_avatar.png");
			
			contactListView.ItemsSource = contacts;
		}
	}
}

