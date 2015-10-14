using System;
using Ninject.Modules;
using Xamarin.Forms;

namespace SocialCapital.Common.FormsMVVM
{
	public class MvvmPattenNinjectModule : NinjectModule
	{
		#region implemented abstract members of NinjectModule
		public override void Load ()
		{
			this.Bind<Func<Page>> ().ToMethod (ctx => PageProxy.GetCurrentPage);

			this.Bind<IDialogProvider> ().To<DialogService> ().InSingletonScope ();
			this.Bind<IPage> ().To<PageProxy> ().InSingletonScope ();
		}
		#endregion
	}
}

