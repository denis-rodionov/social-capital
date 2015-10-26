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
using Ninject;
using SocialCapital.Data.Managers;

namespace SocialCapital.Data
{
	public class DataContext : IDataContext
	{
		private static object locker = new object();

		private static SQLiteConnection connection;
		private static Mutex mutex;

		#region Init

		public DataContext ()
		{
			try
			{
				if (connection == null)
				{					
					connection = DependencyService.Get<ISQLite> ().GetConnection ();
				}

				if (mutex == null)
				{
					mutex = new Mutex(false);
				}

				mutex.WaitOne();

				//Log.GetLogger().Log("DataContext created!", LogLevel.Trace);
			}
			catch (Exception ex) 
			{
				Log.GetLogger ().Log (ex);
				throw;
			}
		}

		#endregion

		#region IDataContext implementation

		public SQLiteConnection Connection { get { return connection; } }

		public void CloseConnection()
		{
			if (connection != null)
			{
				connection.Close ();
				connection = null;
			}
		}

		#endregion

		#region IDisposable implementation
		public void Dispose ()
		{
			mutex.ReleaseMutex ();
			Log.GetLogger ().Log ("DataContext disposed", LogLevel.Trace);
		}
		#endregion


	}
}

