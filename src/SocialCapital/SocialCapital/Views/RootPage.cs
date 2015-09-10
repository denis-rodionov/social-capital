using System;
using Xamarin.Forms;
using SocialCapital.ViewModels;

namespace SocialCapital.Views
{
	public class RootPage : MasterDetailPage
	{
		NavigationVM NavigationModel { get; set; }

		public RootPage ()
		{
			NavigationModel = new NavigationVM (NavigateTo);
			MasterBehavior = MasterBehavior.Popover;

			var menuPage = new MenuPage ();
			menuPage.BindingContext = NavigationModel;

			Master = menuPage;
			Detail = new NavigationPage (NavigationModel.DefaultPage);
		}

		private void NavigateTo(Page page)
		{
			Detail = new NavigationPage (page);

			IsPresented = false;
		}
	}
}


