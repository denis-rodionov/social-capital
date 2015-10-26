using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;

namespace SocialCapital.Views
{
	public partial class SummaryPage : ContentPage
	{
		public SummaryPage (SummaryVM vm)
		{
			InitializeComponent ();
			BindingContext = vm;
		}
	}
}

