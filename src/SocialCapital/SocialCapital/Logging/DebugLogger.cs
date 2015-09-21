using System;
using Xamarin.Forms;

namespace SocialCapital
{
	public class DebugLogger : ICrossPlatformLogger
	{
		public DebugLogger ()
		{
		}

		#region ICrossPlatformLogger implementation

		public void Log (string message, LogLevel level = LogLevel.Info)
		{
			var time = DateTime.Now;
			var log = String.Format ("{0}\t{1}: {2}", time, level, message);
			Debug (log);
		}

		public void Log (Exception ex, LogLevel level = LogLevel.Error)
		{
			var time = DateTime.Now;
			var log = String.Format ("{0}\t{1}: {2}", time, level, ex);
			Debug (log);
		}

		public void Log(string message, Exception ex, LogLevel level = LogLevel.Error)
		{
			var time = DateTime.Now;
			var log = String.Format ("{0}\t{1}: {2}\n{3}", time, level, message, ex);
			Debug (log);
		}

		public void Log(string formattedMessage, params object[] parameters)
		{
			var time = DateTime.Now;
			var log = String.Format ("{0}\t{1}: {2}", time, LogLevel.Info, string.Format(formattedMessage, parameters));
			Debug (log);
		}

		public void Log(LogLevel logLevel, string formattedMessage, params object[] parameters)
		{
			var time = DateTime.Now;
			var log = String.Format ("{0}\t{1}: {2}", time, logLevel, string.Format(formattedMessage, parameters));
			Debug (log);
		}

		#endregion

		private void Debug(string message)
		{
			System.Diagnostics.Debug.WriteLine (message);
		}
	}
}

