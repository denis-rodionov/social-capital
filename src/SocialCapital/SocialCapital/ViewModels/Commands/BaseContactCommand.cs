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
		public Func<IEnumerable<T>> ItemsProvider { get; private set; }

		public string UserInvite { get; private set; }

		public BaseContactCommand (Func<IEnumerable<T>> itemsProvider, string userInviteString)
		{
			ItemsProvider = itemsProvider;
			UserInvite = userInviteString;
		}

		#region ICommand implementation

		public event EventHandler CanExecuteChanged;

		public bool CanExecute (object parameter)
		{
			return ItemsProvider().Count () != 0;
		}

		public async void Execute (object parameter)
		{
			Page page = parameter as Page;
			await CommandAction (page);
		}

		protected abstract Task CommandAction(Page page);

		#endregion

		protected async Task<string> GetValue(Page page)
		{
			var items = ItemsProvider ();

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
			var items = ItemsProvider ();
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
	}
}

