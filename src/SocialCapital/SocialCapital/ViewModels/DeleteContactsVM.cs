using System;
using SocialCapital.Common.FormsMVVM;
using System.Windows.Input;
using Xamarin.Forms;
using SocialCapital.Data.Managers;
using Ninject;

namespace SocialCapital.ViewModels
{
	public class DeleteContactsVM : ContactListVM
	{
		INavigator navigator;

		public DeleteContactsVM (INavigator navigator) :
			base(c => c.DeleteTime.HasValue, true)
		{
			this.navigator = navigator;
		}


	}
}

