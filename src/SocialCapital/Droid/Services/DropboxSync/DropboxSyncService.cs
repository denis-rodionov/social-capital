using System;
using SocialCapital.Services.DropboxSync;
using Xamarin.Forms;
using DropboxSync.Android;
using Android.Content;
using Android.App;

[assembly: Dependency(typeof(SocialCapital.Droid.Services.DropboxSync.DropboxSyncService))]

namespace SocialCapital.Droid.Services.DropboxSync
{
	public class DropboxSyncService : IDropboxSync
	{
		const string AppKey = "pnie92869wlkesv";
		const string AppSecrect = "5sxsv0pw42zxs81";

		public DropboxSyncService ()
		{
			

		}

		private DBAccountManager manager;
		private DBAccountManager Manager {
			get 
			{ 
				if (manager == null)
					manager = DBAccountManager.GetInstance (Forms.Context.ApplicationContext, AppKey, AppSecrect); 

				return manager;
			}
		}

		#region IDropboxSync implementation

		public bool HasDropboxAccount ()
		{
			try
			{
				var account = Manager.LinkedAccount;

				Intent intent = new Intent(Intent.ActionView);
				Manager.StartLink((Activity)Forms.Context, 0);
				account = Manager.LinkedAccount;

				return account != null;
			}
			catch (DropboxException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new DropboxException ("Error occured in getting linked dropbox account", ex);
			}
		}

		public void SaveFile (byte[] data, string fileName)
		{
			try
			{
				var filesystem = GetFileSystem();
				var file = filesystem.Create (new DBPath (fileName));

				if (file == null)
					throw new DropboxException ("Cannot create file!");

				file.WriteStream.Write(data, 0, data.Length);
				file.Close();
			}
			catch (DropboxException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new DropboxException ("Unknown exception in dropbox library. See inner exception", ex);
			}
		}

		public byte[] LoadFile (string fileName)
		{
			try
			{
				var filesystem = GetFileSystem();
				var path = new DBPath(fileName);

				if (!filesystem.Exists(path))
					return null;	// file not created yet
				else
				{
					var file = filesystem.Open (new DBPath (fileName));

					if (file == null)
						throw new DropboxException ("Cannot open file!");

					var length = file.ReadStream.Length;
					byte[] buffer = new byte[length];
					file.ReadStream.Read(buffer, 0, (int)length);
						
					file.Close();

					return buffer;
				}
			}
			catch (DropboxException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new DropboxException ("Unknown exception in dropbox library. See inner exception", ex);
			}
		}

		#endregion

		#region Implementation

		private DBFileSystem GetFileSystem()
		{
			if (Manager.LinkedAccount == null)
				throw new NotLinkedDropboxException ();

			var filesystem = DBFileSystem.ForAccount (Manager.LinkedAccount);

			if (filesystem == null)
				throw new DropboxException ("Filesystem is unavailable");

			return filesystem;
		}

		#endregion
	}
}

