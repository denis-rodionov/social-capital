using System;
using SQLite;
using SQLite.Net;
using Xamarin.Forms;

namespace SocialCapital
{
	public class DataContext : IDisposable
	{
		private static object locker = new object();

		private static SQLiteConnection connection;

		public SQLiteConnection Connection { get { return connection; } }

		#region Init

		public DataContext ()
		{
			try
			{
				if (connection == null)
				{
					connection = DependencyService.Get<ISQLite> ().GetConnection ();

					InitDatabase();
				}
			}
			catch (Exception ex) 
			{
				Log.GetLogger ().Log (ex);
			}
		}

		public void InitDatabase()
		{
			Log.GetLogger ().Log ("Creating tables...");
			connection.CreateTable<Contact> ();
			connection.CreateTable<Tag> ();
			connection.CreateTable<ContactTag> ();
		}

		#endregion


		#region IDisposable implementation
		public void Dispose ()
		{
			
		}
		#endregion
	}
}

