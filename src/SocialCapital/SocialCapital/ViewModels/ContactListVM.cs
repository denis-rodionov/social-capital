using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using SocialCapital.Data;
using Xamarin.Forms;
using SocialCapital.AddressBookImport;
using SocialCapital.Common;
using System.Windows.Input;
using Ninject;
using System.Linq.Expressions;
using SocialCapital.Data.Managers;

namespace SocialCapital.ViewModels
{
	public class ContactListVM : ViewModelBase
	{
		#region Init

		public ContactListVM (Func<Contact, bool> whereClause)
		{
			var timing = Timing.Start ("ContactListVM constructor");

			var manager = App.Container.Get<ContactManager> ();
			var contacts = manager.GetContacts(whereClause);

			FilteredContacts = new ObservableCollection<ContactVM> (contacts.Select(c => new ContactVM(c)));
			SelectCommand = new Command (OnSelectCommandExecuted);

			timing.Finish (LogLevel.Trace);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Contact list
		/// </summary>
		IEnumerable<ContactVM> contacts;
		public IEnumerable<ContactVM> AllContacts{
			get { return contacts; }
		}

		public IEnumerable<ContactVM> FilteredContacts { 
			get { return contacts.Where(c => c.SourceContact.DisplayName.ToLowerInvariant().Contains(Filter.ToLowerInvariant())); }
			set { SetProperty (ref contacts, value); }
		}

		public int ContactsCount {
			get { return FilteredContacts.Count (); }
		}

		/// <summary>
		/// String for filtering contact list
		/// </summary>
		string filter = "";
		public string Filter {
			get { return filter; }
			set { 
				SetProperty (ref filter, value); 
				OnPropertyChanged ("FilteredContacts");
			}
		}

		/// <summary>
		/// Number of contacts selected in multy-select list
		/// </summary>
		public int SelectedCount {
			get { return contacts.Where (c => c.Selected).Count(); }
		}

		public ICommand SelectCommand { get; set; }
		private void OnSelectCommandExecuted(object item)
		{
			var contact = (ContactVM)item;
			contact.Selected = !contact.Selected;
			OnPropertyChanged ("SelectedCount");
		}
			
		#endregion

		public void SelectContacts(IEnumerable<Contact> contacts)
		{
			var ids = contacts.Select (c => c.Id).ToList ();

			foreach (var contact in AllContacts)
				if (ids.Contains(contact.SourceContact.Id))
					contact.Selected = true;
		}

	}
}

