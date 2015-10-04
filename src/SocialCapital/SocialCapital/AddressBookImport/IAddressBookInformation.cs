using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialCapital.AddressBookImport
{
	/// <summary>
	/// Interface for getting address book from dependency service of specific platform
	/// </summary>
	public interface IAddressBookInformation
	{
		IEnumerable<AddressBookContact> GetContacts(long lastTimeStamp = 0);

		/// <summary>
		/// Event of calculation the count of contact in device book
		/// </summary>
		event Action<int> ContactsCountCalculated;

		/// <summary>
		/// Event of the contact retrieve from device book with the count of retrieved contacts
		/// </summary>
		event Action<int> ContactRetrieved;
	}
}

