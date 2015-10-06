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
		readonly Color InactiveColor = new Color (100, 100, 100);
		const double InactiveStatus = -1;

		public ContactStatus (Contact contact, Frequency frequency, DateTime lastContact)
		{
		}

		protected virtual double CalculateRawStatus(Contact contact, Frequency frequency, DateTime lastContact)
		{
			return 1;
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
		public bool Active { get; private set; }

		/// <summary>
		/// Color represents the relationship status
		/// </summary>
		/// <value>The color.</value>
		public Color Color { get; private set; }


	}
}

