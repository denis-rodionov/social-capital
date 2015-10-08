using System;
using SocialCapital.Data.Model;

namespace SocialCapital.Data
{
	public class Settings
	{
		public void SaveValue<T>(string key, T value, DataContext db = null)
		{
			if (db == null)
				using (var innerDb = new DataContext ())
				{
					InnerSaveValue (key, value, db);
				}
			else
				InnerSaveValue (key, value, db);
		}

		private void InnerSaveValue<T>(string key, T value, DataContext db)
		{
			var existingConfig = db.Connection.Table<Config> ().Where (c => c.Key == key).FirstOrDefault ();

			if (existingConfig == null) {
				var newConfig = new Config () { Key = key };
				newConfig.SetValue (value);
				db.Connection.Insert (newConfig);
				Log.GetLogger ().Log (LogLevel.Debug, "Config '{0}' with value '{1}' added to Settings", newConfig.Key, newConfig.Value);
			}
			else {
				existingConfig.SetValue (value);
				db.Connection.Update (existingConfig);
				Log.GetLogger ().Log (LogLevel.Debug, "Config '{0}' updated to value '{1}'", existingConfig.Key, existingConfig.Value);
			}
		}

		public Config GetConfig(string key, DataContext db = null)
		{
			if (db == null)
				using (var innerDb = new DataContext ()) {
					return InnerGetConfig(key, innerDb);
				}
			else
				return InnerGetConfig(key, db);
		}

		private Config InnerGetConfig(string key, DataContext db)
		{
			return db.Connection.Table<Config> ().Where (c => c.Key == key).FirstOrDefault ();
		}

		public T GetConfigValue<T>(string key, DataContext db = null) 
		{
			var config = GetConfig (key, db);

			if (config == null)
				return default(T);
			else
				return config.GetValue<T> ();
		}

		#region Configs

		public const string AddressBookConfig = "LastAddressBookImportTime";
		public DateTime? LastAddressBookImportTime {
			get { return GetConfigValue<DateTime?> (AddressBookConfig); }
			set { SaveValue<DateTime?> (AddressBookConfig, value); }
		}

		public const string MaxUpdatedTimestampConfig = "MaxUpdatedTimestamp";
		public long? MaxUpdatedTimestamp {
			get { return GetConfigValue<long?> (MaxUpdatedTimestampConfig); }
			set { SaveValue<long?> (MaxUpdatedTimestampConfig, value); }
		}

		public const string DatabaseVersionConfig = "DatabaseVersion";
		public string DatabaseVersion {
			get { return GetConfigValue<string> (DatabaseVersionConfig); }
			set { SaveValue<string> (DatabaseVersionConfig, value); }
		}

		#endregion
	}
}

