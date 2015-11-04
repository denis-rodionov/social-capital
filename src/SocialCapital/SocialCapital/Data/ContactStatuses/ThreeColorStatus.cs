using System;
using SocialCapital.Data.Model;
using Xamarin.Forms;

namespace SocialCapital.Data.ContactStatuses
{
	public class ThreeColorStatus : BaseContactStatus
	{
		public ThreeColorStatus (Contact contact, Frequency frequency, CommunicationHistory lastCommunication) :
			base(contact, frequency, lastCommunication)	
		{
		}

		protected override double CalculateRawStatus (Contact contact, Frequency frequency, CommunicationHistory lastCommunication)
		{
			if (contact.Frequent)
				return GreenStatus;

			if (frequency == null || frequency.Never)
				return InactiveStatus;

			if (lastCommunication == null)
				return (GreenStatus - RedStatus) / 2;	// initial status

			var passedSinceCommunication = (DateTime.Now - lastCommunication.Time).TotalDays;
			var slope = -1 / frequency.Period;

			if (passedSinceCommunication > frequency.Period)
				return 0;
			else
				return passedSinceCommunication * slope + 1;
		}

		protected override Color CalculateColor ()
		{
			if (RawStatus > 0.25)
				return GreenColor;
			else if (RawStatus > 0)
				return YellowColor;
			else
				return RedColor;
				
		}
			
	}
}

