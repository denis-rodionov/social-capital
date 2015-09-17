using System;
using System.Windows.Input;
using Xamarin.Forms;
using SocialCapital.Data;

namespace SocialCapital.ViewModels
{
	public class SettingsVM
	{
		public ICommand Erace { get; set; }

		public SettingsVM ()
		{
			Erace = new Command (() => new DataContext ().ClearDatabase ());
		}
	}
}

