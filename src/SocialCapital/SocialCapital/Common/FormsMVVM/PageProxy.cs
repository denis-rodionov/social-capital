using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace SocialCapital.Common.FormsMVVM
{
	public class PageProxy : IPage
	{
		private readonly Func<Page> _pageResolver;

		public PageProxy(Func<Page> pageResolver)
		{
			_pageResolver = pageResolver;
		}

		public async Task DisplayAlert(string title, string message, string cancel)
		{
			await _pageResolver().DisplayAlert(title, message, cancel);
		}

		public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
		{
			return await _pageResolver().DisplayAlert(title, message, accept, cancel);
		}

		public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
		{
			return await _pageResolver().DisplayActionSheet(title, cancel, destruction, buttons);
		}

		public INavigation Navigation
		{
			get { return _pageResolver().Navigation; }
		}

		public static Page GetCurrentPage()
		{
			// Check if we are using MasterDetailPage
			var masterDetailPage = App.Current.MainPage as MasterDetailPage;

			var page = masterDetailPage != null 
				? masterDetailPage.Detail 
				: App.Current.MainPage;

			// Check if page is a NavigationPage
			var navigationPage = page as IPageContainer<Page>;

			return navigationPage != null 
				? navigationPage.CurrentPage
					: page;
		}
	}
}

