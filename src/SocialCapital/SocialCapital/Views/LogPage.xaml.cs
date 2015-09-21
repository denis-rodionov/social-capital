using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;

namespace SocialCapital.Views
{
	public partial class LogPage : ContentPage
	{
		public LogPage (LogViewModel viewModel)
		{
			BindingContext = viewModel;

			InitializeComponent ();
		}
	}
}

