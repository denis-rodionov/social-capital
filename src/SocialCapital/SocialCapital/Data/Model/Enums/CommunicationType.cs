using System;

namespace SocialCapital.Data.Model.Enums
{
	/// <summary>
	/// Communication types: All thre types of communication:
	/// NotAnswere - when we send something by sms or mail and not get answer
	/// Answered - complete communication
	/// Asked - when we were writed and not answered yet
	/// </summary>
	public enum CommunicationType {
		PhoneCall = 1,
		EmailSent = 2,
		SmsSend = 3
	}
}

