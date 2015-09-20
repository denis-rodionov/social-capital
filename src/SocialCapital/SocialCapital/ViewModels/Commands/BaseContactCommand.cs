using System;
using System.Collections.Generic;
using SocialCapital.Data.Model;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;

namespace SocialCapital
{
	public abstract class BaseContactCommand<T> : ICommand where T : ILabeled
	{
		public Func<IEnumerable<T>> GetItems { get; private set; }

		public string UserInvite { get; private set; }

		public Contact Contact { get; private set; }

		public event Action CommandExecuted;

		public BaseContactCommand (Contact contact, Func<IEnumerable<T>> getItems, string userInviteString)
		{
			UserInvite = userInviteString;
			Contact = contact;
			GetItems = getItems;
		}

		protected abstract Task<bool> CommandAction(Page page);

		protected abstract void SaveCommunication ();

		#region ICommand implementation

		public event EventHandler CanExecuteChanged;

		public bool CanExecute (object parameter)
		{
			return GetItems().Count () != 0;
		}

		public async void Execute (object parameter)
		{
			if (GetItems().Count () == 0)
			{
				CanExecuteChangedRaise ();
				throw new ArgumentException("No items in the list (phones, emails)");
			}

			Page page = parameter as Page;
			var executed = await CommandAction (page);

			if (executed)
			{
				SaveCommunication ();
				RaiseCommandExecuted ();
			}
		}

		#endregion

		protected async Task<string> GetValue(Page page)
		{
			var items = GetItems();

			if (items == null || items.Count () == 0)
				throw new Exception ("Contact does not have a phone number");

			string number;
			if (items.Count () > 1) {
				var phone = await ChooseItem (page);

				if (phone == null)	// user canceled operation
					return string.Empty;

				number = phone.GetValue();
			} else
				number = items.Single ().GetValue();

			return number;
		}

		protected async Task<T> ChooseItem(Page page)
		{
			var items = GetItems();
			var dict = new Dictionary<string, T> ();

			foreach (var item in items)
				dict.Add (string.Format ("{0} : {1}", item.GetLabel(), item.GetValue()), item);

			var label = await page.DisplayActionSheet (UserInvite, 
				AppResources.CancelButton, 
				null,
				dict.Keys.ToArray());

			if (label == AppResources.CancelButton)
				return default(T);
			else
				return dict [label];
		}

		private void CanExecuteChangedRaise()
		{
			var handler = CanExecuteChanged;
			if (handler != null)
				CanExecuteChanged (this, new EventArgs());
		}

		private void RaiseCommandExecuted()
		{
			var handler = CommandExecuted;
			if (handler != null)
				CommandExecuted();
		}
	}
}

