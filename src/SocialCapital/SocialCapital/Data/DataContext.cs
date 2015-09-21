using System;
using SQLite;
using SQLite.Net;
using Xamarin.Forms;
using SocialCapital.Data.Model;
using SocialCapital.Common;
using System.Linq;
using System.Collections.Generic;
using SocialCapital.Logging;
using System.Threading;

namespace SocialCapital.Data
{
	public class DataContext : IDisposable
	{
		private static object locker = new object();

		private static SQLiteConnection connection;
		private static Mutex mutex;

		public SQLiteConnection Connection { get { return connection; } }

		#region Init

		public DataContext ()
		{
			try
			{
				if (connection == null)
				{
					connection = DependencyService.Get<ISQLite> ().GetConnection ();
					mutex = new Mutex(false);
				}

				mutex.WaitOne();
			}
			catch (Exception ex) 
			{
				Log.GetLogger ().Log (ex);
			}
		}

		public static void InitDatabase()
		{
			using (var db = new DataContext ())
			{
				db.Connection.CreateTable<Contact> ();
				db.Connection.CreateTable<Tag> ();
				db.Connection.CreateTable<ContactTag> ();
				db.Connection.CreateTable<Frequency> ();
				db.Connection.CreateTable<Phone> ();
				db.Connection.CreateTable<Email> ();
				db.Connection.CreateTable<Address> ();
				db.Connection.CreateTable<Config> ();
				db.Connection.CreateTable<ContactModification> ();
				db.Connection.CreateTable<CommunicationHistory> ();
				db.Connection.CreateTable<LogMessage> ();
			}
		}

		public static void ClearDatabase()
		{
			using (var db = new DataContext ())
			{
				db.Connection.DeleteAll<Contact> ();
				db.Connection.DeleteAll<Tag> ();
				db.Connection.DeleteAll<ContactTag> ();
				db.Connection.DeleteAll<Frequency> ();
				db.Connection.DeleteAll<Phone> ();
				db.Connection.DeleteAll<Email> ();
				db.Connection.DeleteAll<Address> ();
				db.Connection.DeleteAll<Config> ();
				db.Connection.DeleteAll<ContactModification> ();
				db.Connection.DeleteAll<CommunicationHistory> ();
				db.Connection.DeleteAll<LogMessage> ();
			}
		}

		public static IEnumerable<LogMessage> GetLogs()
		{
			using (var db = new DataContext())
			{
				return db.Connection.Table<LogMessage>().ToList();
			}
		}

		#endregion

		#region IDisposable implementation
		public void Dispose ()
		{
			mutex.ReleaseMutex ();
		}
		#endregion


	}
}

