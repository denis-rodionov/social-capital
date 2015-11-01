using System;
using SocialCapital.Data.Model;
using Xamarin.Forms;

namespace SocialCapital.Common
{
	/// <summary>
	/// Represent how match wanted frequency of communication with actual one
	/// Calculated by given frequency and last contact time
	/// </summary>
	public class ContactStatus 
	{
		readonly Color InactiveColor = Color.Silver;
		readonly Color GreenColor = new Color(0, 100, 0);
		readonly Color RedColor = new Color(100, 0, 0);
		const double InactiveStatus = -1;
		const double RedStatus = 0;
		const double GreenStatus = 1;

		public ContactStatus (Contact contact, Frequency frequency, CommunicationHistory lastCommunication)
		{
			RawStatus = CalculateRawStatus (contact, frequency, lastCommunication);
		}

		protected virtual double CalculateRawStatus(Contact contact, Frequency frequency, CommunicationHistory lastCommunication)
		{
			if (contact.Frequent)
				return GreenStatus;

			if (frequency == null || frequency.Never)
				return InactiveStatus;

			if (lastCommunication == null)
				return (GreenStatus - RedStatus) / 2;

			var passedSinceCommunication = (DateTime.Now - lastCommunication.Time).TotalDays;
			var middlePoint = frequency.Period / 2;
			var slope = -1 / (frequency.Period - middlePoint);

			if (passedSinceCommunication < middlePoint)
				return GreenStatus;
			else
				return passedSinceCommunication * (slope + frequency.Period);
		}

		/// <summary>
		/// Double representation. Range between 0 and 1.
		/// 1 - Communication took place not a long time ago
		/// 0 - No communication for a long time (relatively long: according the frequency specified for the given contact)
		/// -1 - for contact which we are not interested to communicate
		/// </summary>
		public double RawStatus { get; private set; }

		/// <summary>
		/// Determin if the user interested in the developing relationship with the person.
		/// </summary>
		/// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
		public bool Active { get { return RawStatus != InactiveStatus; }}

		/// <summary>
		/// Color represents the relationship status
		/// </summary>
		/// <value>The color.</value>
		public Color Color { 
			get  {
				if (RawStatus == InactiveStatus)
					return InactiveColor;

				var r = RedColor.R + (int)((GreenColor.R - RedColor.R) * RawStatus);
				var g = RedColor.G + (int)((GreenColor.G - RedColor.G) * RawStatus);
				var b = RedColor.B + (int)((GreenColor.B - RedColor.B) * RawStatus);

				return Color.FromRgb(r, g, b);
			}
		}
	}
}

