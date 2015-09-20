using System;
using SQLite.Net.Attributes;
using SocialCapital.Data.Model.Enums;

namespace SocialCapital.Data.Model
{
	public class CommunicationHistory
	{
		/// <summary>
		/// Primary ID
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		/// <summary>
		/// Contact the communication is related to
		/// </summary>
		public int ContactId { get; set; }

		/// <summary>
		/// Communication type: phone, sms....
		/// </summary>
		public CommunicationType Type { get; set; }

		/// <summary>
		/// Date and time of the communication
		/// </summary>
		/// <value>The time.</value>
		public DateTime Time { get; set; }

		/// <summary>
		/// Short message of communication.
		/// The first string of the FullMessage for example
		/// </summary>
		public string ShortMessage { get; set; }

		/// <summary>
		/// The full text of the communication.
		/// In case of phone call: null
		/// </summary>
		public string FullMessage { get; set; }
	}
}

