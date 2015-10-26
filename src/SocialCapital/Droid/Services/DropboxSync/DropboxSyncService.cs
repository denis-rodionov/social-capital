using System;
using SocialCapital.Services.DropboxSync;
using Xamarin.Forms;
using DropboxSync.Android;
using Android.Content;
using Android.App;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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

				if (account == null)
				{
					Manager.StartLink((Activity)Forms.Context, 0);
					account = Manager.LinkedAccount;
				}

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

		public void UploadFile (byte[] data, string dropboxFileName)
		{
			try
			{
				var filesystem = GetFileSystem();
				var path = new DBPath (dropboxFileName);
				DBFile file;

				if (filesystem.Exists(path))
					filesystem.Delete(path);
				
				file = filesystem.Create (path);

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

		public void UploadFile (string path, string dropboxFileName)
		{
			try
			{
				var data = File.ReadAllBytes(path);

				UploadFile(data, dropboxFileName);
			}
			catch (DropboxException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new DropboxException ("Unknown exception while preparing file to upload", ex);
			}
		}

		public byte[] DownloadData (string fileName)
		{
			try
			{
				var filesystem = GetFileSystem();
				var path = new DBPath(fileName);

				if (!filesystem.Exists(path))
					throw new DropboxException("Backup file not found in dropbox");
				else
				{
					var file = filesystem.Open (new DBPath (fileName));

					if (file == null)
						throw new DropboxException ("Cannot open file!");
					
					byte[] buffer = ReadFully(file.ReadStream);

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

		public void DownloadFile (string fileName, string destinationPath)
		{
			try
			{
				var data = DownloadData(fileName);

				File.WriteAllBytes(destinationPath, data);
			}
			catch (DropboxException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new DropboxException ("Exception while processing downloaded file", ex);
			}
		}

		public IEnumerable<DropboxFile> GetFileInfos()
		{
			try
			{
				var filesystem = GetFileSystem();

				var files = filesystem.ListFolder(DBPath.Root);

				var res = files.Select(f => new DropboxFile() { Name = ToFileName(f.Path), Modified = ToDateTime(f.ModifiedTime) }).ToList();

				return res;
			}
			catch (DropboxException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new DropboxException ("Error while accessin dropbox files lise", ex);
			}
		}

		#endregion

		#region Implementation

		public static byte[] ReadFully (Stream stream)
		{
			byte[] buffer = new byte[32768];
			using (MemoryStream ms = new MemoryStream())
			{
				while (true)
				{
					int read = stream.Read (buffer, 0, buffer.Length);
					if (read <= 0)
						return ms.ToArray();
					ms.Write (buffer, 0, read);
				}
			}
		}

		private string ToFileName(DBPath path)
		{
			var res = path.ToString ().Replace ("/", "");
			return res;
		}

		private DateTime ToDateTime(Java.Util.Date date)
		{
			return new DateTime (date.Year, date.Month, date.Day, date.Hours, date.Minutes, date.Seconds);
		}

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

