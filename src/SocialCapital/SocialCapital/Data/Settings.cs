using System;
using SocialCapital.Data.Model;

namespace SocialCapital.Data
{
	public class Settings
	{
		const string AddressBookConfig = "LastAddressBookImportTime";

		public DateTime? LastAddressBookImportTime {
			get { return GetConfigValue<DateTime?> (AddressBookConfig); }
			set { SaveValue<DateTime?> (AddressBookConfig, value); }
		}

		public void SaveValue<T>(string key, T value)
		{
			using (var db = new DataContext ()) {
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
		}

		public Config GetConfig(string key)
		{
			using (var db = new DataContext ()) {
				return db.Connection.Table<Config> ().Where (c => c.Key == key).FirstOrDefault ();
			}
		}

		public T GetConfigValue<T>(string key) 
		{
			var config = GetConfig (key);

			if (config == null)
				return default(T);
			else
				return config.GetValue<T> ();
		}
	}
}

