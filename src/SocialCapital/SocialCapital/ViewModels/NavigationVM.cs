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
			get { return new ContactListPage () { BindingContext = new ContactListVM (c => true) }; }
		}

		AddressBookVM addressBook;
		public AddressBookVM AddressBookViewModel {
			get {
				if (addressBook == null)
					addressBook = new AddressBookVM ();

				return addressBook;
			}
		}

		SettingsVM settings;
		public SettingsVM SettingsViewModel {
			get {
				if (settings == null)
					settings = new SettingsVM ();

				return settings;
			}
		}

		private Page GetPageByName(string name)
		{
			switch (name) {
				case "AllContacts":
					return new ContactListPage ();
				case "AddressBookImport":
					return new AddressBookImportPage () { BindingContext = AddressBookViewModel };
				case "Settings":
					return new SettingsPage (SettingsViewModel);
				case "ContactsProcessing":
					return new GroupsPage(new ContactGroupListVM());
				case "PriorityContactList":
					return new PriorityContactListPage (new PriorityContactListVM ());
				default:
					return new ContentPage ();
			}
		}
	}
}

