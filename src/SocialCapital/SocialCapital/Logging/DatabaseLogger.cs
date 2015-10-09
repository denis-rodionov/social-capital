using System;
using SocialCapital.Data;
using SocialCapital.Data.Model;
using Ninject;
using SocialCapital.Data.Managers;

namespace SocialCapital.Logging
{
	public class DatabaseLogger : ICrossPlatformLogger
	{
		public DatabaseLogger ()
		{
		}

		#region ICrossPlatformLogger implementation

		public void Log (string message, LogLevel level = LogLevel.Info)
		{
			var log = new LogMessage () { Time = DateTime.Now, Level = level, Message = message };

		}

		public void Log (Exception ex, LogLevel level = LogLevel.Error)
		{
			var log = new LogMessage () { Time = DateTime.Now, Level = level, Message = ex.ToString() };
			App.Container.Get<LogManager> ().SaveLog (log);
		}

		public void Log (string message, Exception ex, LogLevel level = LogLevel.Error)
		{
			var log = new LogMessage () { Time = DateTime.Now, Level = level, Message = string.Format("{0}\n{1}", message, ex.ToString()) };
			App.Container.Get<LogManager> ().SaveLog (log);
		}

		public void Log (string formattedMessage, params object[] parameters)
		{
			var log = new LogMessage () { Time = DateTime.Now, Level = LogLevel.Info, Message = string.Format(formattedMessage, parameters) };
			App.Container.Get<LogManager> ().SaveLog (log);
		}

		public void Log (LogLevel logLevel, string formattedMessage, params object[] parameters)
		{
			var log = new LogMessage () { Time = DateTime.Now, Level = logLevel, Message = string.Format(formattedMessage, parameters) };
			App.Container.Get<LogManager> ().SaveLog (log);
		}

		#endregion
	}
}

