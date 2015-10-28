using System;
using SocialCapital;
using Xamarin.Forms;
using SocialCapital.Droid;
using System.IO;
using SocialCapital.Data;
using Android.Renderscripts;
using System.Text;

[assembly:Dependency(typeof (SQLite_Android))]

namespace SocialCapital.Droid
{
	public class SQLite_Android : ISQLite
	{
		public SQLite_Android ()
		{
		}

		#region ISQLite implementation
		public SQLite.Net.SQLiteConnection GetConnection ()
		{
			var sqliteFilename = "database.db3";
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			var path = Path.Combine(documentsPath, sqliteFilename);

			//var file = new FileInfo (path);
			//var folders = GetAllFoldersWithSizes ();

			var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid ();
			var connection = new SQLite.Net.SQLiteConnection (platform, path, false);

			// Return the database connection 
			return connection;
		}
		#endregion

		static string GetAllFoldersWithSizes()
		{
			StringBuilder builder = new StringBuilder ();

			foreach (var item in Enum.GetValues(typeof(System.Environment.SpecialFolder)))
			{
				var path = System.Environment.GetFolderPath ((Environment.SpecialFolder)item);

				if (Directory.Exists (path))
				{
					var size = GetDirectorySize (path); 
					builder.AppendLine (string.Format ("{0}\t{1}\t{2} KB", item, path, size / 1024));
				}
			}

			return builder.ToString ();
		}

		static long GetDirectorySize(string p)
		{
			// 1.
			// Get array of all file names.
			string[] a = Directory.GetFiles(p, "*.*");

			// 2.
			// Calculate total bytes of all files in a loop.
			long b = 0;
			foreach (string name in a)
			{
				// 3.
				// Use FileInfo to get length of each file.
				FileInfo info = new FileInfo(name);
				b += info.Length;
			}
			// 4.
			// Return total size
			return b;
		}

	}
}

