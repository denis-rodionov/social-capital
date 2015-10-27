using System;
using SocialCapital.Data;
using System.Threading.Tasks;
using Xamarin.Forms;
using SocialCapital.Common.EventProviders;
using SocialCapital.Services.FileService;
using SocialCapital.Common;

namespace SocialCapital.Services.DropboxSync
{
	public class DropboxBackupService
	{
		public const string SettingsLastBackupKey = "DropboxLastBackupTime";
		const string SettingsRestoreTime = "DropboxRestoreTime";
		const string SettingsKey = "DropboxSyncConfig";
		const string DropboxFileName = "backup.bak";
		const string DropboxOldBackupFileName = "oldbackup.bak";
		const string DbCopyLocalPath = "dbBackup.bak";
		static readonly TimeSpan AfterRestoreDelay = TimeSpan.FromSeconds (5);

		public bool Enabled { get; private set; }

		private object guard = new object();
		private Settings settings;
		private IDropboxSync dropboxService;
		private IEventProvider eventProvider;
		private Func<IDataContext> dbFactory;
		private IFileService fileService;
		private DatabaseService databaseService;

		public DropboxBackupService (IDropboxSync dropboxService, 
									Settings settings, 
									IEventProvider eventProvider, 
									Func<IDataContext> dbFactory,
									IFileService fileService,
									DatabaseService databaseService)
		{
			lock (guard)
			{
				this.settings = settings;
				this.dropboxService = dropboxService;
				this.eventProvider = eventProvider;
				this.dbFactory = dbFactory;
				this.fileService = fileService;
				this.databaseService = databaseService;

				this.eventProvider.Raised += DoBackup ;

				Enabled = settings.GetConfigValue<bool> (SettingsKey);
				if (Enabled)
					StartSyncWorker (dropboxService);
			}
		}

		#region public

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

		public DropboxFile GetBackupFile()
		{
			var files = dropboxService.GetFileInfos ();

			foreach (var file in files)
				if (file.Name == DropboxFileName)
					return file;

			return null;
		}

		public DropboxFile GetBackupFromAnotherDevice()
		{
			var backup = GetBackupFile ();
			var lastBackupTime = settings.GetConfigValue<DateTime> (SettingsLastBackupKey);

			var diff = backup.Modified - lastBackupTime;

			if (diff.TotalMinutes >= 1)
				return backup;
			else
				return null;
		}

		public bool HasDropboxAccount()
		{
			return dropboxService.HasDropboxAccount ();
		}

		public void Restore(DropboxFile backup)
		{
			var logger = Log.GetLogger ();
			using (var db = dbFactory())
			{
				logger.Log ("Restoging database...", LogLevel.Info);

				// create backup of current db
				var dbPath = db.Connection.DatabasePath;
				var backupPath = BackupDatabase (db);

				// close db connection
				db.CloseConnection();

				logger.Log (string.Format ("Backup on device created on path={0}, dbpath={1}", backupPath, dbPath), LogLevel.Trace);

				// upload current db to dropbox
				dropboxService.UploadFile(backupPath, DropboxOldBackupFileName);

				logger.Log ("Backup uploaded to dropbox", LogLevel.Trace);;

				// delete current db
				fileService.Delete(dbPath);

				// download backup file to db path 
				dropboxService.DownloadFile(backup.Name, dbPath);

				databaseService.InitDatabase ();

				settings.SaveValue (SettingsRestoreTime, DateTime.Now);

				logger.Log ("Database restored");
			}
		}

		#endregion

		#region private

		private string BackupDatabase(IDataContext db)
		{
			var timing = Timing.Start ("Database backup");

			// var path = db.Connection.CreateDatabaseBackup (db.Connection.Platform);

			var dbPath = db.Connection.DatabasePath;

			db.CloseConnection ();

			var folderPath = GetFolderPath (dbPath);
			var copyFile = folderPath + DbCopyLocalPath;

			fileService.Copy (dbPath, copyFile);

			timing.Finish (LogLevel.Debug);

			return copyFile;
		}

		private string GetFolderPath(string filePath)
		{
			var length = filePath.LastIndexOf ("/") + 1;
			var res = filePath.Substring (0, length);
			return res;
		}

		private void StartSyncWorker(IDropboxSync dropbox)
		{
			eventProvider.Start ();
		}

		private void StopSyncWorker()
		{
			eventProvider.Stop ();
		}

		private void DoBackup()
		{
			Task.Run (() => InnerDoBackup ());
		}

		private void InnerDoBackup()
		{
			try
			{
				Log.GetLogger ().Log ("Dropbox backup creating...");

				string backupPath;
				using (var db = dbFactory())
				{
					backupPath = BackupDatabase (db);
				}

				Log.GetLogger ().Log ("Backup created on device: " + backupPath, LogLevel.Trace);

				dropboxService.UploadFile(backupPath, DropboxFileName);

				Log.GetLogger ().Log ("Uploaded to dropbox", LogLevel.Trace);

				// delete backup from device
				fileService.Delete (backupPath);

				settings.SaveValue (SettingsLastBackupKey, DateTime.Now);

				Log.GetLogger ().Log ("Backup created in dropbox", LogLevel.Trace);
			}
			catch (Exception ex)
			{
				Log.GetLogger ().Log (ex);
			}
		}

		#endregion
	}
}

