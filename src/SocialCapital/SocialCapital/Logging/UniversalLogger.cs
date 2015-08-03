using System;
using System.Diagnostics;

namespace SocialCapital
{
	public class UniversalLogger : ICrossPlatformLogger
	{
		public UniversalLogger ()
		{
		}

		#region ICrossPlatformLogger implementation

		public void Log (string message, LogLevel level = LogLevel.Info)
		{
			var time = DateTime.Now;
			var log = String.Format ("{0}\t{1}: {2}", time, level, message);
			Debug.WriteLine (log);
		}

		public void Log (Exception ex, LogLevel level = LogLevel.Error)
		{
			var time = DateTime.Now;
			var log = String.Format ("{0}\t{1}: {2}", time, level, ex);
			Debug.WriteLine (log);
		}

		public void Log(string message, Exception ex, LogLevel level = LogLevel.Error)
		{
			var time = DateTime.Now;
			var log = String.Format ("{0}\t{1}: {2}\n{3}", time, level, message, ex);
			Debug.WriteLine (log);
		}

		#endregion
	}
}

