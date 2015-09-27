using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;

namespace SocialCapital.Views
{
	public partial class GroupDetailsPage : ContentPage
	{
		public GroupDetailsPage (ContactGroupVM vm)
		{
			InitializeComponent ();
			BindingContext = vm;
		}
	}
}

