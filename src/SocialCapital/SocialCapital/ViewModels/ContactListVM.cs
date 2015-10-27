using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using SocialCapital.Data;
using Xamarin.Forms;
using SocialCapital.Services.AddressBookImport;
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
			var contactModels = manager.GetContacts (whereClause).ToList ();

			contacts = new List<ContactVM> (contactModels.Select(c => new ContactVM(c)));
			SelectCommand = new Command (OnSelectCommandExecuted);

			timing.Finish (LogLevel.Trace);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Contact list
		/// </summary>
		List<ContactVM> contacts;
		public IEnumerable<ContactVM> AllContacts{
			get { return contacts; }
		}

		public IEnumerable<ContactVM> FilteredContacts { 
			get { 
				var filter = Filter.ToLowerInvariant ();
				var byName = contacts.Where(c => c.SourceContact.DisplayName.ToLowerInvariant().Contains(filter));
				var byTags = contacts.Where (c => c.Tags.TagList.ToLowerInvariant ().Contains (filter));

				return byName.Union(byTags);
			}
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

		#region callbacks

		public void OnDeletedContact(ContactVM contact)
		{
			try
			{
				contacts.Remove(contact);
				OnPropertyChanged("FilteredContacts");
			}
			catch (Exception ex)
			{
				Log.GetLogger ().Log (ex);
			}
		}

		#endregion

	}
}

