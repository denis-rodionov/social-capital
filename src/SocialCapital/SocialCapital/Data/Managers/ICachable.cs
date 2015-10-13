using System;

namespace SocialCapital.Data.Managers
{
	public interface ICachable
	{
		void RefreshCache(IDataContext db = null);
		void ClearCache();
	}
}

