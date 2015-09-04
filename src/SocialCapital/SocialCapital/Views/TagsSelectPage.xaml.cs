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

		public async void OnDeleteItem(object sender, EventArgs args)
		{
			var vm = BindingContext as TagsVM;
			var tag = (sender as MenuItem).CommandParameter as Tag;

			vm.Tags.Remove (tag);
		}
	}
}

