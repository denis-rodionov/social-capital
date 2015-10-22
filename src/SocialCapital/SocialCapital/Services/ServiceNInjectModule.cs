using System;
using Ninject.Modules;
using SocialCapital.Services.DropboxSync;
using Xamarin.Forms;
using SocialCapital.Common.EventProviders;
using SocialCapital.Services.FileService;

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
			Bind<IFileService> ().ToMethod (ctx => DependencyService.Get<IFileService> ());
			Bind<DropboxBackupService> ().ToSelf ().InSingletonScope ();
			Bind<IEventProvider> ().To<TimerEventProvider> ();
		}

		#endregion
	}
}

