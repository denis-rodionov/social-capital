using System;

namespace SocialCapital.Data.Managers
{
	public interface ICachable
	{
		void RefreshCache(DataContext db = null);
		void ClearCache();
	}
}

