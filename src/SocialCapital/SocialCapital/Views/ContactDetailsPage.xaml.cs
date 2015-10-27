using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SocialCapital.ViewModels;
using SocialCapital.Common.FormsMVVM;
using Ninject;

namespace SocialCapital.Views
{
	public partial class ContactDetailsPage : ContentPage
	{
		public ContactDetailsPage ()
		{
			InitializeComponent ();
		}

		public void OnEditMenu(object sender, EventArgs EventArgs)
		{
			var editPage = new ContactEditPage () {
				BindingContext = this.BindingContext
			};

			Navigation.PushModalAsync (new NavigationPage(editPage));
		}

		public async void OnDeleteMenu(object sender, EventArgs args)
		{
			var vm = (ContactVM)BindingContext;

			var dialogService = App.Container.Get<IDialogProvider> ();

			var yes = await dialogService.DisplayAlert (AppResources.DeleteNoun, AppResources.SureToDeleteContactQuestion, AppResources.Yes, AppResources.No);

			if (yes)
			{
				vm.DeleteContact ();
				Navigation.PopAsync ();	
			}
		}
	}


}

