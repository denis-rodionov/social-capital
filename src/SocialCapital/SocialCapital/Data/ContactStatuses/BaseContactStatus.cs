using System;
using SocialCapital.Data.Model;
using Xamarin.Forms;

namespace SocialCapital.Data.ContactStatuses
{
	/// <summary>
	/// Represent how match wanted frequency of communication with actual one
	/// Calculated by given frequency and last contact time
	/// </summary>
	public abstract class BaseContactStatus 
	{
		protected readonly Color InactiveColor = Color.Silver;
		protected readonly Color GreenColor;
		protected readonly Color RedColor;
		protected readonly Color YellowColor;
		protected const double InactiveStatus = -1;
		protected const double RedStatus = 0;
		protected const double GreenStatus = 1;

		public BaseContactStatus (Contact contact, Frequency frequency, CommunicationHistory lastCommunication)
		{
			RawStatus = CalculateRawStatus (contact, frequency, lastCommunication);
			GreenColor = Color.FromHex ("#43DB3B");
			RedColor = new Color (100, 0, 0);
			YellowColor = Color.Yellow;
		}

		protected abstract double CalculateRawStatus (Contact contact, Frequency frequency, CommunicationHistory lastCommunication);

		protected abstract Color CalculateColor ();

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

				return CalculateColor ();
			}
		}		
	}
}

