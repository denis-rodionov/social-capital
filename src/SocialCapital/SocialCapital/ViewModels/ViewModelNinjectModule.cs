using System;

using Xamarin.Forms;
using Ninject.Modules;
using SocialCapital.Common.FormsMVVM;

namespace SocialCapital.ViewModels
{
	public class ViewModelNinjectModule : NinjectModule
	{
		#region implemented abstract members of NinjectModule
		public override void Load ()
		{
			this.Bind<ContactListVM> ().To<ContactListVM> ();
			this.Bind<AddressBookVM> ().To<AddressBookVM> ().InSingletonScope ();
			this.Bind<SettingsVM> ().ToSelf ();
			this.Bind<SummaryVM> ().ToSelf ();
			this.Bind<TagsVM> ().ToSelf ();
			this.Bind<ContactDetailsVM> ().ToSelf ();
			this.Bind<DeleteContactsVM> ().ToSelf ();
		}
		#endregion
	}
}


