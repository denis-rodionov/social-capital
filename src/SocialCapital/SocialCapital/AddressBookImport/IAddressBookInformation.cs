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
		Task<List<AddressBookContact>> GetContacts();
	}
}

