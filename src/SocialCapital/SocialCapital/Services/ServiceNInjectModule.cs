using System;
using Ninject.Modules;
using SocialCapital.Services.DropboxSync;
using Xamarin.Forms;
using SocialCapital.Common.EventProviders;
using SocialCapital.Services.FileService;
using Ninject;

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

			Bind<DropboxBackupService> ().ToSelf ().InSingletonScope ().WithConstructorArgument (
				typeof(IEventProvider),
				ctx => ctx.Kernel.Get<IEventProvider> ("BackupTimer"));

			Bind<IEventProvider> ().To<TimerEventProvider> ().Named("BackupTimer")
				.WithConstructorArgument (typeof(TimeSpan), TimeSpan.FromSeconds (20));
		}

		#endregion
	}
}

