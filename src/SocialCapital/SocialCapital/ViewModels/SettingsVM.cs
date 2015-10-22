using System;
using System.Windows.Input;
using Xamarin.Forms;
using SocialCapital.Data;
using SocialCapital.Views;
using SocialCapital.Services.DropboxSync;

namespace SocialCapital.ViewModels
{
	public class SettingsVM
	{
		private DropboxBackupService dropboxSyncker;
		private IDropboxSync dropboxService;

		public SettingsVM (DropboxBackupService dropboxSyncker, IDropboxSync dropboxService)
		{
			this.dropboxService = dropboxService;
			this.dropboxSyncker = dropboxSyncker;

			IsDropboxAccountLinked = dropboxService.HasDropboxAccount();
		}

		#region Properties

		public bool IsDropboxAccountLinked { get; set; }

		#endregion

		#region Commands

		public void SyncEnable(bool value)
		{
			if (value)
				dropboxSyncker.Start ();
			else
				dropboxSyncker.Stop ();
		}

		#endregion
	}
}

