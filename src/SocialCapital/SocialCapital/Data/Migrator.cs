using System;
using System.Collections.Generic;
using System.Reflection;
using SocialCapital.Data.Migrations;
using System.Linq;

namespace SocialCapital.Data
{
	public class Migrator
	{
		public const string DatabaseVersionConfig = "DatabaseVersion";

		private readonly ISettings settings;
		private readonly string databaseVersion;
		private readonly List<IMigration> migrations;

		public Migrator(ISettings settings, string databaseVersion, IEnumerable<IMigration> migrations)
		{
			this.settings = settings;
			this.databaseVersion = databaseVersion;
			this.migrations = migrations.ToList();
		}

		public void Migrate(IDataContext db)
		{
			if (db == null)
				throw new ArgumentException ("db is null");

			var deviceVersion = settings.GetConfigValue<string> (DatabaseVersionConfig, db);

			if (deviceVersion == null)
				settings.SaveValue (DatabaseVersionConfig, databaseVersion, db);
			else if (deviceVersion != databaseVersion)
			{
				InnerMigrate (deviceVersion, db);
				settings.SaveValue (DatabaseVersionConfig, databaseVersion, db);
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

