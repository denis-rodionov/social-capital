using System;

namespace SocialCapital.Data
{
	public interface ISettings
	{
		void SaveValue<T> (string key, T value, IDataContext db = null);

		T GetConfigValue<T> (string key, IDataContext db = null);
	}
}

