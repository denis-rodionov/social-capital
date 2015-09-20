﻿using System;
using SQLite;
using SQLite.Net;
using Xamarin.Forms;
using SocialCapital.Data.Model;
using SocialCapital.Common;

namespace SocialCapital.Data
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
					var timing = Timing.Start("DataContext  Init");
					connection = DependencyService.Get<ISQLite> ().GetConnection ();

					InitDatabase();

					timing.Finish(LogLevel.Trace);
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
			connection.CreateTable<Frequency> ();
			connection.CreateTable<Phone> ();
			connection.CreateTable<Email> ();
			connection.CreateTable<Address> ();
			connection.CreateTable<Config> ();
			connection.CreateTable<ContactModification> ();
			connection.CreateTable<CommunicationHistory> ();
		}

		public void ClearDatabase()
		{
			connection.DeleteAll<Contact> ();
			connection.DeleteAll<Tag> ();
			connection.DeleteAll<ContactTag> ();
			connection.DeleteAll<Frequency> ();
			connection.DeleteAll<Phone> ();
			connection.DeleteAll<Email> ();
			connection.DeleteAll<Address> ();
			connection.DeleteAll<Config> ();
			connection.DeleteAll<ContactModification> ();
		}

		#endregion


		#region IDisposable implementation
		public void Dispose ()
		{
			
		}
		#endregion
	}
}

