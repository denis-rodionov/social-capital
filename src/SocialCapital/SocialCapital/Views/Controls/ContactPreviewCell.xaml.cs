using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SocialCapital.ViewModels;
using SocialCapital.Views.Controls;

namespace SocialCapital.Views.Controls
{
	public partial class ContactPreviewCell : FastCell
	{
		protected override void InitializeCell ()
		{
			InitializeComponent ();	
		}

		protected override void SetupCell (bool isRecycled)
		{
			var vm = (ContactVM)BindingContext;

			if (vm != null)
			{
				ContactName.Text = vm.FullName;
				ColorStatus.Color = vm.ContactStatus.Color;
				ContactPhoto.Source = vm.PhotoImage;

				if (!string.IsNullOrEmpty (vm.GroupName))
				{
					GroupName.Text = vm.GroupName;
					GroupNameElement.IsVisible = true;
				}
				
				TagList.Text = vm.Tags.TagList;
			}
		}
	}
}

