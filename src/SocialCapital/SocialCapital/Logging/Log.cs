using System;

namespace SocialCapital
{
	public class Log
	{
		public static ICrossPlatformLogger GetLogger ()
		{
			return new UniversalLogger ();
		}
	}
}

