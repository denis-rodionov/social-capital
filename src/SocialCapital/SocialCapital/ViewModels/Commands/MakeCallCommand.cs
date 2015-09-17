using System;
using System.Windows.Input;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using SocialCapital.PhoneServices;
using System.Threading.Tasks;

namespace SocialCapital.ViewModels.Commands
{
	public class MakeCallCommand : PhoneCommand, ICommand
	{
		public MakeCallCommand (IEnumerable<Phone> phones) : base(phones)
		{
		}

		#region ICommand implementation

		public event EventHandler CanExecuteChanged;

		public bool CanExecute (object parameter)
		{
			return Phones.Count () != 0;
		}

		public async void Execute (object parameter)
		{
			Page page = parameter as Page;
			await MakeCall (page);
		}

		#endregion

		public async Task MakeCall(Page page)
		{
			if (Phones == null || Phones.Count () == 0)
				throw new Exception ("Contact does not have a phone number");

			string number;
			if (Phones.Count () > 1) {
				var phone = await ChoosePhone (page);

				if (phone == null)	// user canceled operation
					return;
				
				number = phone.Number;
			} else
				number = Phones.Single ().Number;
			
			DependencyService.Get<IPhoneService> ().Call (number);
		}


	}
}

