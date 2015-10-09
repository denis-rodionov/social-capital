using System;
using SQLite.Net.Attributes;
using SocialCapital.Data.Model.Enums;

namespace SocialCapital.Data.Model
{
	public class LogMessage : IHaveId, IEquatable<LogMessage>
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

		#region IEquatable implementation

		public bool Equals (LogMessage other)
		{
			return Id != other.Id;
		}

		public override int GetHashCode ()
		{
			return Id.GetHashCode ();
		}

		#endregion
	}
}

