using System;
using SocialCapital.Data.Model;

namespace SocialCapital.Data
{
	public class Settings
	{
		public DateTime? LastAddressBookImportTime {
			get { return GetConfigValue<DateTime?> ("LastAddressBookImportTime"); }
		}

		public void SaveValue<T>(string key, T value)
		{
			var config = new Config () {
				Key = key
			};
			config.SetValue (value);

			using (var db = new DataContext ()) {
				db.Connection.InsertOrReplace (config);
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

