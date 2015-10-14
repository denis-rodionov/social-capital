using System;
using System.Collections.Generic;
using SocialCapital.Data.Model;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
using SocialCapital.Data.Model.Enums;
using SocialCapital.Common.FormsMVVM;

namespace SocialCapital
{
	public abstract class BaseActionCommand<T> : ICommand where T : ILabeled
	{
		public Func<IEnumerable<T>> GetItems { get; private set; }

		public string UserInvite { get; private set; }

		public Contact Contact { get; private set; }

		protected IDialogProvider dialogService;

		public BaseActionCommand (Contact contact, Func<IEnumerable<T>> getItems, string userInviteString, IDialogProvider dialogService)
		{
			UserInvite = userInviteString;
			Contact = contact;
			GetItems = getItems;
			this.dialogService = dialogService;
		}

		protected abstract Task<bool> CommandAction(string number);

		protected abstract void SaveCommunication ();

		#region ICommand implementation

		public event EventHandler CanExecuteChanged;
		public event Action CommandExecuted;

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
			var executed = await InnerAction ();

			if (executed)
			{
				SaveCommunication ();
				RaiseCommandExecuted ();
			}
		}

		private async Task<bool> InnerAction()
		{
			string number = await GetValue ();

			// user canceled the operation
			if (number == string.Empty)
				return false;

			var executed = await CommandAction (number);
			if (!executed)
				ShowError ();
			
			return executed;
		}

		private async void ShowError()
		{
			await dialogService.DisplayAlert (AppResources.Error,
				AppResources.NoPermissions,
				AppResources.CancelButton);
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

		#endregion

		protected async Task<string> GetValue()
		{
			var items = GetItems();

			if (items == null || items.Count () == 0)
				throw new Exception ("Contact does not have a phone number");

			string number;
			if (items.Count () > 1) {
				var phone = await ChooseItem ();

				if (phone == null)	// user canceled operation
					return string.Empty;

				number = phone.GetValue();
			} else
				number = items.Single ().GetValue();

			return number;
		}

		protected async Task<T> ChooseItem()
		{
			var items = GetItems();
			var dict = new Dictionary<string, T> ();

			foreach (var item in items)
				dict.Add (string.Format ("{0} : {1}", item.GetLabel(), item.GetValue()), item);

			var label = await dialogService.DisplayActionSheet (UserInvite,
				            AppResources.CancelButton,
				            null,
				            dict.Keys.ToArray ());

			if (label == AppResources.CancelButton)
				return default(T);
			else
				return dict [label];
		}


	}
}

