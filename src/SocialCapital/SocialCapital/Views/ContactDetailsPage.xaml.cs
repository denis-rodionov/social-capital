using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SocialCapital.ViewModels;
using SocialCapital.Common.FormsMVVM;
using Ninject;
using System.Threading.Tasks;
using SocialCapital.Data.Model;
using SocialCapital.Data.Managers;

namespace SocialCapital.Views
{
	public partial class ContactDetailsPage : ContentPage
	{
		ContactVM vm;

		public ContactDetailsPage (ContactVM contactModel)
		{
			this.vm = contactModel;

			InitializeComponent ();

			BindingContext = vm;
		}

		public void OnEditMenu(object sender, EventArgs EventArgs)
		{			
			var editVM = vm.CreateEditVM ();

			editVM.Saved += c => {
				vm.Tags = null;
				vm.FullName = c.DisplayName;
			};

			var editPage = new ContactEditPage (editVM);

			Navigation.PushModalAsync (new NavigationPage(editPage));
		}

		public async void OnDeleteMenu(object sender, EventArgs args)
		{
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

