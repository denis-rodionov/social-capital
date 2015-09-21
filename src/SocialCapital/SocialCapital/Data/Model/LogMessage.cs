using System;
using SQLite.Net.Attributes;

namespace SocialCapital.Data.Model
{
	public class LogMessage
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public LogLevel Level { get; set; }

		public string Message { get; set; }

		public DateTime Time { get; set; }

		public string Summary
		{
			get {
				return string.Format ("{0}: {1}", Time, Message);			
			}
		}
	}
}

