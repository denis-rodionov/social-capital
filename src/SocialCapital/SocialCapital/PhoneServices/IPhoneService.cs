using System;

namespace SocialCapital.PhoneServices
{
	public interface IPhoneService
	{
		bool Call(string number);

		bool WriteSmS (string number, string smsBody = "");

		bool SendEmail(string toAddress, string subject = "", string text = "");
	}
}

