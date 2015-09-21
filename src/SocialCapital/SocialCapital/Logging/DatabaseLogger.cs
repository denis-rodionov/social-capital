using System;
using SocialCapital.Data;
using SocialCapital.Data.Model;

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
			using (var db = new DataContext ())
			{
				var log = new LogMessage () { Time = DateTime.Now, Level = level, Message = message };
				InsertLog (log, db);
			}
		}

		public void Log (Exception ex, LogLevel level = LogLevel.Error)
		{
			using (var db = new DataContext ())
			{
				var log = new LogMessage () { Time = DateTime.Now, Level = level, Message = ex.ToString() };
				InsertLog (log, db);
			}
		}

		public void Log (string message, Exception ex, LogLevel level = LogLevel.Error)
		{
			using (var db = new DataContext ())
			{
				var log = new LogMessage () { Time = DateTime.Now, Level = level, Message = string.Format("{0}\n{1}", message, ex.ToString()) };
				InsertLog (log, db);
			}
		}

		public void Log (string formattedMessage, params object[] parameters)
		{
			using (var db = new DataContext ())
			{
				var log = new LogMessage () { Time = DateTime.Now, Level = LogLevel.Info, Message = string.Format(formattedMessage, parameters) };
				InsertLog (log, db);
			}
		}

		public void Log (LogLevel logLevel, string formattedMessage, params object[] parameters)
		{
			using (var db = new DataContext ())
			{
				var log = new LogMessage () { Time = DateTime.Now, Level = logLevel, Message = string.Format(formattedMessage, parameters) };
				InsertLog (log, db);
			}
		}

		#endregion

		void InsertLog (LogMessage log, DataContext db)
		{
			db.Connection.Insert (log);
			if (log.Id == 0)
				throw new Exception ("Log is not saved to database\n" + log.Message);
		}
	}
}

