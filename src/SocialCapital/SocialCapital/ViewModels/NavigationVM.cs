using System;
using Xamarin.Forms;
using System.Windows.Input;
using SocialCapital.Views;

namespace SocialCapital.ViewModels
{
	public class MenuItem {
		public string Name { get; set; }
		public string Title { get; set; }
		public string IconSource { get; set; }
	}

	public class NavigationVM
	{
		Action<Page> NavigateTo { get; set; }

		public ICommand NavigateCommand { get; set; }

		public NavigationVM (Action<Page> navigateTo)
		{
			NavigateTo = navigateTo;

			NavigateCommand = new Command (MenuItemSelected);
		}

		public void MenuItemSelected(object item)
		{
			var menuItem = (MenuItem)item;

			var page = GetPageByName (menuItem.Name);

			NavigateTo (page);
		}

		public Page DefaultPage {
			get { return new ContactListPage (); }
		}

		private Page GetPageByName(string name)
		{
			switch (name) {
				case "AllContacts":
					return new ContactListPage ();
				default:
					return new ContentPage ();
			}
		}
	}
}

