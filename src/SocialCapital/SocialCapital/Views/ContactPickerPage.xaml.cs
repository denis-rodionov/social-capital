using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SocialCapital.ViewModels;
using SocialCapital.Data.Model;
using System.Linq;

namespace SocialCapital.Views
{
	public partial class ContactPickerPage : ContentPage
	{
		Action<IEnumerable<Contact>> DelegateOnDone = null;

		public ContactPickerPage (ContactListVM vm, Action<IEnumerable<Contact>> OnDone)
		{
			InitializeComponent ();
			BindingContext = vm;
			DelegateOnDone = OnDone;
		}

		protected void OnDoneClicked(object sender, EventArgs args)
		{
			var vm = (ContactListVM)BindingContext;

			if (DelegateOnDone != null)
				DelegateOnDone (vm.AllContacts.Where(c => c.Selected).Select(c => c.SourceContact).ToList());
			
			Navigation.PopModalAsync ();
		}
	}
}

