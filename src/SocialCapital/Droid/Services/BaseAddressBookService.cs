using System;

namespace SocialCapital.Droid.Services
{
	public class BaseAddressBookService
	{
		const int ProgressReportFrequency = 2;

		protected int CountRetrieved { get; set; }

		/// <summary>
		/// Event of calculation the count of contact in device book
		/// </summary>
		public event Action<int> ContactsCountCalculated;

		/// <summary>
		/// Event of the contact retrieve from device book with the count of retrieved contacts
		/// </summary>
		public event Action<int> ContactRetrieved;

		public BaseAddressBookService ()
		{
		}

		protected void RaiseCountCalculated(int count)
		{
			var handler = ContactsCountCalculated;

			if (handler != null)
				handler (count);
		}

		protected void RaiseContactRetrieved(int count)
		{
			if (count % ProgressReportFrequency == 0) {
				var handler = ContactRetrieved;

				if (handler != null)
					handler (count);
			}

		}
	}
}

