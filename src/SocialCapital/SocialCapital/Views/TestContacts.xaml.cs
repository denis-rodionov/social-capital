using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;

namespace SocialCapital.Views
{
	public partial class TestContacts : ContentPage
	{
		public TestContacts (TestContactsVM vm)
		{
			InitializeComponent ();
			BindingContext = vm;
		}
	}
}

