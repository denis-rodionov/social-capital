using System;
using SocialCapital.Data;
using System.Threading.Tasks;
using Xamarin.Forms;
using SocialCapital.Common.EventProviders;
using SocialCapital.Services.FileService;

namespace SocialCapital.Services.DropboxSync
{
	public class DropboxBackupService
	{
		const string SettingsKey = "DropboxSyncConfig";
		const string DropboxFileName = "backup.bak";

		public bool Enabled { get; private set; }

		private object guard = new object();
		private Settings settings;
		private IDropboxSync dropboxService;
		private IEventProvider eventProvider;
		private Func<IDataContext> dbFactory;
		private IFileService fileService;

		public DropboxBackupService (IDropboxSync dropboxService, 
									Settings settings, 
									IEventProvider eventProvider, 
									Func<IDataContext> dbFactory,
									IFileService fileService)
		{
			lock (guard)
			{
				this.settings = settings;
				this.dropboxService = dropboxService;
				this.eventProvider = eventProvider;
				this.dbFactory = dbFactory;
				this.fileService = fileService;

				this.eventProvider.Raised += DoSync ;

				Enabled = settings.GetConfigValue<bool> (SettingsKey);
				if (Enabled)
					StartSyncWorker (dropboxService);
			}
		}

		public void Start()
		{
			lock (guard)
			{
				if (!Enabled)
				{
					StartSyncWorker (dropboxService);
					Enabled = true;
					settings.SaveValue (SettingsKey, true);
				}
			}
		}

		public void Stop()
		{
			lock (guard)
			{
				if (Enabled)
				{
					StopSyncWorker ();
					Enabled = false;
					settings.SaveValue (SettingsKey, false);
				}
			}
		}

		private void StartSyncWorker(IDropboxSync dropbox)
		{
			eventProvider.Start ();
		}

		private void StopSyncWorker()
		{
			eventProvider.Stop ();
		}

		private void DoSync()
		{
			Log.GetLogger ().Log ("Dropbox synchronization...");

			string backupPath;
			using (var db = dbFactory())
			{
				backupPath = db.Connection.CreateDatabaseBackup (db.Connection.Platform);
			}

			Log.GetLogger ().Log ("Backup created: " + backupPath, LogLevel.Trace);

			dropboxService.UploadFile(backupPath, DropboxFileName);

			Log.GetLogger ().Log ("Uploaded to dropbox", LogLevel.Trace);

			// delete backup from device
			fileService.Delete (backupPath);

			Log.GetLogger ().Log ("Synckronized with the dropbox", LogLevel.Trace);
		}
	}
}

