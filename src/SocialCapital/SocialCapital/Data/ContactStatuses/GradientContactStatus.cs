using System;
using SocialCapital.Data.Model;
using Xamarin.Forms;

namespace SocialCapital.Data.ContactStatuses
{
	public class GradientContactStatus : BaseContactStatus
	{
		public GradientContactStatus (Contact contact, Frequency frequency, CommunicationHistory lastCommunication) :
			base(contact, frequency, lastCommunication)	
		{
		}

		protected override double CalculateRawStatus(Contact contact, Frequency frequency, CommunicationHistory lastCommunication)
		{
			if (contact.Frequent)
				return GreenStatus;

			if (frequency == null || frequency.Never)
				return InactiveStatus;

			if (lastCommunication == null)
				return (GreenStatus - RedStatus) / 2;	// initial status

			var passedSinceCommunication = (DateTime.Now - lastCommunication.Time).TotalDays;
			var middlePoint = frequency.Period / 2;
			var slope = -1 / (frequency.Period - middlePoint);

			if (passedSinceCommunication < middlePoint)
				return GreenStatus;
			else if (passedSinceCommunication > frequency.Period)
				return 0;
			else
				return passedSinceCommunication * (slope + frequency.Period);
		}

		protected override Color CalculateColor ()
		{
			var r = RedColor.R + ((GreenColor.R - RedColor.R) * RawStatus);
			var g = RedColor.G + ((GreenColor.G - RedColor.G) * RawStatus);
			var b = RedColor.B + ((GreenColor.B - RedColor.B) * RawStatus);
			return Color.FromRgb (r, g, b);
		}
	}
}

