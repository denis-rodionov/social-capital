using System;
using Ninject.Modules;
using SocialCapital.Services.DropboxSync;
using Xamarin.Forms;

namespace SocialCapital.Services
{
	public class ServiceNInjectModule : NinjectModule
	{
		public ServiceNInjectModule ()
		{
		}

		#region implemented abstract members of NinjectModule

		public override void Load ()
		{
			Bind<IDropboxSync> ().ToMethod (ctx => DependencyService.Get<IDropboxSync> ());
			Bind<DropboxSyncker> ().ToSelf ().InSingletonScope ();
		}

		#endregion
	}
}

