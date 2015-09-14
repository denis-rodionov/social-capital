using System;
using SocialCapital;
using Xamarin.Forms;
using SocialCapital.Droid;
using System.IO;
using SocialCapital.Data;

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

			var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid ();
			var connection = new SQLite.Net.SQLiteConnection (platform, path);

			// Return the database connection 
			return connection;
		}
		#endregion

	}
}

