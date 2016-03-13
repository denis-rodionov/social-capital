using System;
using Xamarin.Forms;
using System.Windows.Input;
using SocialCapital.Views;
using Ninject;

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

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="navigateTo">Delegate to navigate to details pages</param>
		public NavigationVM (Action<Page> navigateTo)
		{
			NavigateTo = navigateTo;

			NavigateCommand = new Command (MenuItemSelected);
		}

		/// <summary>
		/// Menu Selected handler
		/// </summary>
		/// <param name="item">Selected item</param>
		public void MenuItemSelected(object item)
		{
			var menuItem = (MenuItem)item;

			var page = GetPageByName (menuItem.Name);

			NavigateTo (page);
		}

		public Page DefaultPage {
			get { 
				//return new ContactListPage () { BindingContext = new ContactListVM (c => true) }; 
				return new PriorityContactListPage (new PriorityContactListVM ());
			}
		}

		private Page GetPageByName(string name)
		{
			switch (name) {
				case "AllContacts":
					return new ContactListPage ();
				case "AddressBookImport":
					return new AddressBookImportPage () { BindingContext = App.Container.Get<AddressBookVM>() };
				case "Settings":
					return App.Container.Get<SettingsPage>();
				case "ContactsProcessing":
					return new GroupsPage(new ContactGroupListVM());
				case "PriorityContactList":
					return new PriorityContactListPage (new PriorityContactListVM ());
				case "Summary":
					return App.Container.Get<SummaryPage> (); 
				default:
					return new ContentPage ();
			}
		}
	}
}

