using System;
using SocialCapital.Data;

namespace SocialCapital.Services.DropboxSync
{
	public class DropboxSyncker
	{
		const string SettingsKey = "DropboxSyncConfig";

		public bool Enabled { get; private set; }

		private object guard = new object();
		private Settings settings;
		private IDropboxSync dropboxService;

		public DropboxSyncker (IDropboxSync dropboxService, Settings settings)
		{
			lock (guard)
			{
				this.settings = settings;
				this.dropboxService = dropboxService;

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
					settings.SaveValue (SettingsKey, Enabled);
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
					settings.SaveValue (SettingsKey, Enabled);
				}
			}
		}

		private void StartSyncWorker(IDropboxSync dropbox)
		{
			// do sync work ...
		}

		private void StopSyncWorker()
		{
			// do stop work ...
		}
	}
}

