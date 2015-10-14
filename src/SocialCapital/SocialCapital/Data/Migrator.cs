using System;
using System.Collections.Generic;
using System.Reflection;
using SocialCapital.Data.Migrations;

namespace SocialCapital.Data
{
	public class Migrator
	{
		const string databaseVersion = "0.2";

		private Settings settings;

		private List<IMigration> migrations;

		public Migrator(Settings settings)
		{
			this.settings = settings;

			migrations = new List<IMigration> () {
				new Migration_0_1 ()
			};
		}

		public void Migrate(IDataContext db)
		{
			if (db == null)
				throw new ArgumentException ("db is null");

			var deviceVersion = settings.GetConfigValue<string> (Settings.DatabaseVersionConfig, db);

			if (deviceVersion == null)
				settings.SaveValue (Settings.DatabaseVersionConfig, databaseVersion, db);
			else if (deviceVersion != databaseVersion)
			{
				InnerMigrate (deviceVersion, db);
				settings.SaveValue (Settings.DatabaseVersionConfig, databaseVersion, db);
			}
		}

		private void InnerMigrate(string deviceVersion, IDataContext db)
		{
			foreach (var mig in migrations)
				if (IsMigrationActual (mig, deviceVersion))
				{
					mig.Migrate (db);
					Log.GetLogger ().Log (string.Format ("Migration '{0}' made", mig.Version));
				}
		}

		private bool IsMigrationActual(IMigration migration, string deviceVersion)
		{
			int migVersion = VersionToInt (migration.Version);
			int devVersion = VersionToInt (deviceVersion);

			return migVersion > devVersion;
		}

		private int VersionToInt(string version)
		{
			string[] parsed = version.Split ('.');
			return int.Parse (parsed [0]) * 1000 + int.Parse (parsed [1]);
		}
	}
}

