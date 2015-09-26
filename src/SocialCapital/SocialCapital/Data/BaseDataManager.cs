using System;
using SocialCapital.Data.Model.Enums;

namespace SocialCapital.Data
{
	public abstract class BaseDataManager
	{
		public BaseDataManager ()
		{
		}

		public void CheckId(IHasId dbObject)
		{
			if (dbObject.Id == 0)
				throw new Exception(string.Format("Database object {0} has incorrect id == 0", dbObject));
		}
	}
}

