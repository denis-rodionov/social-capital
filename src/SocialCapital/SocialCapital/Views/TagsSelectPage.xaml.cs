using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SocialCapital.ViewModels;
using SocialCapital.Data.Model;
using System.Threading.Tasks;
using System.Threading;

namespace SocialCapital.Views
{
	public partial class TagsSelectPage : ContentPage
	{
		public TagsSelectPage ()
		{
			InitializeComponent ();
		}

		public void OnPop(object sender, EventArgs e)
		{
			Navigation.PopAsync ();
		}
	}
}

