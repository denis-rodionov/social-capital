using System;
using System.Windows.Input;
using Xamarin.Forms;
using SocialCapital.Data;
using SocialCapital.Views;
using SocialCapital.Services.DropboxSync;
using SocialCapital.Common.FormsMVVM;
using System.Threading.Tasks;

namespace SocialCapital.ViewModels
{
	public class SettingsVM : ViewModelBase
	{
		private DropboxBackupService dropboxSyncker;
		private IDialogProvider dialogProvider;

		public SettingsVM (DropboxBackupService dropboxSyncker,
						   IDialogProvider dialogProvider)
		{
			this.dropboxSyncker = dropboxSyncker;
			this.dialogProvider = dialogProvider;

			//IsDropboxAccountLinked = dropboxSyncker.HasDropboxAccount();
			dropboxBackupEnabled = dropboxSyncker.Enabled;
		}

		#region Properties

		private bool isDropboxAcountLinked = false;
		public bool IsDropboxAccountLinked { 
			get { return isDropboxAcountLinked; }
			set { SetProperty (ref isDropboxAcountLinked, value); }
		}

		private bool dropboxBackupEnabled = false;
		public bool DropboxBackupEnabled { 
			get { return dropboxBackupEnabled; } 
			set { 
				if (SetProperty (ref dropboxBackupEnabled, value))
					SyncEnable (value);
			}
		}

		private bool dropboxSync = false;
		public bool DropboxSync {
			get { return dropboxSync; } 
			set { SetProperty (ref dropboxSync, value); }
		}

		#endregion

		#region Commands

		private async void SyncEnable(bool value)
		{
			if (value)	// on
			{
				var hasAccount = dropboxSyncker.HasDropboxAccount();
				if (!hasAccount)
				{
					DropboxBackupEnabled = false;
					return;
				}
				
				var backup = dropboxSyncker.GetBackupFile ();

				// if backup already exists
				if (backup != null)
				{					
					// ask about restore
					var yes = await dialogProvider.DisplayAlert (AppResources.RestoreBackupQuestion,
						string.Format ("{0} {1}", AppResources.RestoreBackupQuestionDetails, backup.Modified.ToAgoFormatRus ()),
						AppResources.RestoreBackup,
						AppResources.CancelButton);
					
					if (yes)
					{
						await Task.Run (() => RestoreDatabase (backup));

						dropboxSyncker.Start ();
					} else
					{
						DropboxBackupEnabled = false;
					}
				}
				else
					dropboxSyncker.Start ();
			}
			else
				dropboxSyncker.Stop ();
		}

		#endregion

		#region private

		private void RestoreDatabase(DropboxFile backup)
		{
			DropboxSync = true;
			dropboxSyncker.Restore (backup);
			DropboxSync = false;
		}

		#endregion
	}
}

