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
				} else
					GroupNameElement.IsVisible = false;
				
				//TagList.Text = vm.Tags.TagList;
				if (vm.Tags != null && vm.Tags.Tags.Count != 0)
				{
					TagsControl.Tags = vm.Tags.Tags;
					TagsControl.IsVisible = true;
				} else
					TagsControl.IsVisible = false;

				// event icon
				if (vm.BirthdateToday)
					EventIcon.Source = ImageSource.FromFile ("dropbox_logo_36dp.png");
				else
					EventIcon.Source = null;
			}
		}
	}
}

