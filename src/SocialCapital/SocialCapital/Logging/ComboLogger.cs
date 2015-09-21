using System;
using System.Threading.Tasks;

namespace SocialCapital.Logging
{
	public class ComboLogger : ICrossPlatformLogger
	{
		DebugLogger debugLogger = null;
		DatabaseLogger databaseLogger = null;

		public ComboLogger ()
		{
			debugLogger = new DebugLogger ();
			databaseLogger = new DatabaseLogger ();
		}

		#region ICrossPlatformLogger implementation

		public void Log (string message, LogLevel level = LogLevel.Info)
		{
			debugLogger.Log(message, level);
			Task.Run (() =>	databaseLogger.Log (message, level));
		}

		public void Log (Exception ex, LogLevel level = LogLevel.Error)
		{
			debugLogger.Log(ex, level);
			Task.Run (() =>	databaseLogger.Log (ex, level));
		}

		public void Log (string message, Exception ex, LogLevel level = LogLevel.Error)
		{
			debugLogger.Log(message, ex, level);
			Task.Run (() =>	databaseLogger.Log (message, ex, level));
		}

		public void Log (string formattedMessage, params object[] parameters)
		{
			debugLogger.Log(formattedMessage, parameters);
			Task.Run (() =>	databaseLogger.Log (formattedMessage, parameters));
		}

		public void Log (LogLevel logLevel, string formattedMessage, params object[] parameters)
		{
			debugLogger.Log(logLevel, formattedMessage, parameters);
			Task.Run (() =>	databaseLogger.Log (logLevel, formattedMessage, parameters));
		}

		#endregion
	}
}

