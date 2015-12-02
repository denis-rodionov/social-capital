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

			this.Bind<IViewFactory> ().To<ViewFactory> ().InSingletonScope ();

			this.Bind<INavigator> ().To<Navigator> ().InSingletonScope ();

			this.Bind<Func<INavigation>> ().ToMethod (ctx => () => App.Current.MainPage.Navigation);

			Bind(typeof (Lazy<INavigation>)).ToMethod(
				ctx => new Lazy<INavigation>(() => PageProxy.GetCurrentPage().Navigation));
		}
		#endregion
	}
}

