using System;

namespace SocialCapital.Data.Managers
{
	public class DataManagerException : Exception
	{
		public DataManagerException (string msg) :  base(msg)
		{
		}
	}
}

