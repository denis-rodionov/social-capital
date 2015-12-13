using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SocialCapital.ViewModels;
using SocialCapital.Views.Converters;
using System.ComponentModel;
using SocialCapital.Common.FormsMVVM;
using Ninject;
using System.Threading.Tasks;

namespace SocialCapital.Views.Controls
{
	public partial class ChoosableContactCell : FastCell
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
				//Subscribe (vm);

				ContactName.Text = vm.FullName;
				ContactPhoto.Source = vm.PhotoImage;

				//TagList.Text = vm.Tags.TagList;
				if (vm.Tags != null && vm.Tags.Tags.Count != 0)
				{
					TagsControl.Tags = vm.Tags.Tags;
					TagsControl.IsVisible = true;
				} else
					TagsControl.IsVisible = false;

				UpdateCheckBox (vm.Selected);
			}
		}

		private void UpdateCheckBox(bool val)
		{
			CheckBoxImage.Source = (string)new BoolToImageConverter ().Convert (val, typeof(string), null, null);
		}

		private void OnTapped(object sender, EventArgs e)
		{
			var vm = (ContactVM)BindingContext;
			if (vm != null)
			{
				vm.Selected = !vm.Selected;
				UpdateCheckBox (vm.Selected);
			}
		}

		private async void OnEditMenuClicked(object sender, EventArgs e)
		{
			var vm = new ContactDetailsVM ((ContactVM)BindingContext);
			if (vm != null)
			{
				var page = new ContactDetailsPage (vm);
				var parentPage = PageProxy.GetCurrentPage ();

				await Task.Yield ();
				parentPage.Navigation.PushAsync (page);
			}
		}

		private void Subscribe(INotifyPropertyChanged vm)
		{
			//vm.PropertyChanged -= OnViewModelChanged;
			//vm.PropertyChanged += OnViewModelChanged;
		}

		private void OnViewModelChanged(object sender, EventArgs e)
		{			
			SetupCell (true);
		}

		private void OnCellTapped(object sender, EventArgs e)
		{
			var vm = (ContactVM)BindingContext;

			if (vm != null)
			{
				vm.Selected = !vm.Selected;
				CheckBoxImage.Source = (string)new BoolToImageConverter ().Convert (vm.Selected, typeof(string), null, null);
			}
		}
	}
}

