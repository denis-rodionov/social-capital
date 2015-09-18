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
		public IEnumerable<T> Items { get; private set; }

		public string UserInvite { get; private set; }

		public BaseContactCommand (IEnumerable<T> items, string userInviteString)
		{
			Items = items;
			UserInvite = userInviteString;
		}

		#region ICommand implementation

		public event EventHandler CanExecuteChanged;

		public bool CanExecute (object parameter)
		{
			return Items.Count () != 0;
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
			if (Items == null || Items.Count () == 0)
				throw new Exception ("Contact does not have a phone number");

			string number;
			if (Items.Count () > 1) {
				var phone = await ChooseItem (page);

				if (phone == null)	// user canceled operation
					return string.Empty;

				number = phone.GetValue();
			} else
				number = Items.Single ().GetValue();

			return number;
		}

		protected async Task<T> ChooseItem(Page page)
		{
			var dict = new Dictionary<string, T> ();

			foreach (var item in Items)
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

