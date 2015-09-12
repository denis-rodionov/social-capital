using System;

namespace SocialCapital
{
	public enum LogLevel 
	{
		Trace,
		Debug,
		Info,
		Error,
		Critical
	}

	public interface ICrossPlatformLogger
	{
		void Log(string message, LogLevel level = LogLevel.Info);

		void Log(Exception ex, LogLevel level = LogLevel.Error);

		void Log(string message, Exception ex, LogLevel level = LogLevel.Error);

		void Log(string formattedMessage, params object[] parameters);

		void Log(LogLevel logLevel, string formattedMessage, params object[] parameters);
	}
}

