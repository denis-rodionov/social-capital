using System;
using SocialCapital.Data;
using System.Collections.Generic;
using System.Linq;

namespace SocialCapital.Tests.Data.Mocks
{
	public class SettingsMock : ISettings
	{
		public Dictionary<string, object> Dict = new Dictionary<string, object> ();

		#region ISettings implementation
		public void SaveValue<T> (string key, T value, IDataContext db = null)
		{
			if (Dict.Keys.SingleOrDefault (k => k == key) == null)
				Dict.Add (key, value);
			else
				Dict [key] = value;
		}
		public T GetConfigValue<T> (string key, IDataContext db = null)
		{
			if (Dict.Keys.SingleOrDefault (k => k == key) != null)
				return (T)Dict [key];
			else
				return default(T);
		}
		#endregion
		
	}
}

