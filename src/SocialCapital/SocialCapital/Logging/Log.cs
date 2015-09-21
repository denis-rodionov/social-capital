using System;
using SocialCapital.Logging;

namespace SocialCapital
{
	public class Log
	{
		static ComboLogger Logger = new ComboLogger();

		public static ICrossPlatformLogger GetLogger ()
		{
			return Logger;
		}
	}
}

