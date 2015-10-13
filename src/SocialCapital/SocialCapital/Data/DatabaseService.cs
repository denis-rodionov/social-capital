using System;
using Ninject;
using System.Collections.Generic;
using SocialCapital.Data.Managers;
using SocialCapital.Common;
using SocialCapital.Data.Model;

namespace SocialCapital.Data
{
	public class DatabaseService
	{
		const string databaseVersion = "0.1";

		private Func<IDataContext> contextFactory;
		private Settings settings;

		public DatabaseService (Func<IDataContext> contextFactory, Settings settings)
		{
			this.contextFactory = contextFactory;
			this.settings = settings;
		}

		public void InitDatabase()
		{
			var timing = Timing.Start ("InitDatabase");

			using (var db = contextFactory())
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
				db.Connection.CreateTable<Group> ();

				if (db.Connection.Table<Frequency> ().Count () == 0)
					App.Container.Get<GroupsManager> ().Init ();

				// load cache
				foreach (var manager in GetAllDataManagers())
					manager.RefreshCache (db);

				CheckVersion(db);
			}

			timing.Finish (LogLevel.Trace);
		}

		public void ClearDatabase()
		{
			using (var db = contextFactory())
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
				db.Connection.DeleteAll<Group> ();
			}

			// Clear cache
			foreach (var manager in GetAllDataManagers())
				manager.ClearCache ();
		}

		public void CheckVersion(IDataContext db)
		{
			var ver = App.Container.Get<Settings> ().GetConfigValue<string> (Settings.DatabaseVersionConfig, db);

			if (ver == null)
				settings.SaveValue (Settings.DatabaseVersionConfig, databaseVersion, db);
			else if (ver != databaseVersion)
				throw new Exception(string.Format("Database version is different: AppVersion={0}, DeviceVersion={1}", databaseVersion, ver));
		}



		public IEnumerable<ICachable> GetAllDataManagers()
		{
			return new List<ICachable> () {
				App.Container.Get<ContactManager> (),
				App.Container.Get<CommunicationManager> (),
				App.Container.Get<EmailManager> (),
				App.Container.Get<FrequencyManager> (),
				App.Container.Get<GroupsManager> (),
				App.Container.Get<ModificationManager> (),
				App.Container.Get<PhonesManager> (),
				App.Container.Get<LogManager>(),
				App.Container.Get<TagManager>(),
				App.Container.Get<ContactTagsManager>()
			};
		}
	}
}

