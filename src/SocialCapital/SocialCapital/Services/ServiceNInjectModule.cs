using System;
using Ninject.Modules;
using SocialCapital.Services.DropboxSync;
using Xamarin.Forms;
using SocialCapital.Common.EventProviders;
using SocialCapital.Services.FileService;
using Ninject;
using SocialCapital.Data;

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
			Bind<IFileService> ().ToMethod (ctx => DependencyService.Get<IFileService> ());

			LoadDropboxBackupService ();
		}

		#endregion

		#region Implementation

		private void LoadDropboxBackupService()
		{
			Bind<IDropboxSync> ().ToMethod (ctx => DependencyService.Get<IDropboxSync> ());

			Bind<DropboxBackupService> ().ToSelf ().InSingletonScope ().WithConstructorArgument (
				typeof(IEventProvider),
				ctx => ctx.Kernel.Get<IEventProvider> ("BackupTimer"));

			Bind<IEventProvider> ().To<TimerEventProvider> ().Named ("BackupTimer")
				.WithConstructorArgument (typeof(TimeSpan), TimeSpan.FromSeconds (20))
				.WithConstructorArgument (typeof(DateTime), 
					ctx => ctx.Kernel.Get<Settings> ().GetConfigValue<DateTime> (DropboxBackupService.SettingsLastBackupKey));
		}

		#endregion
	}
}

