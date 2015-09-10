using System;
using System.Collections.Generic;
using SocialCapital.Data.Model;

namespace SocialCapital.AddressBookImport
{
	public class AddressBookContact
	{
		public string Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string DisplayName { get; set; }

		public string MiddleName { get; set; }

		public string NickName { get; set; }

		public string Prefix { get; set; }

		public string Suffix { get; set; }

		public bool IsAggregate { get; set; }

		public byte[] Thumbnail { get; set; }

		public IEnumerable<Organization> Organizations { get; set; }

		public IEnumerable<Phone> Phones { get; set; }

		public IEnumerable<Email> Emails { get; set; }

		public IEnumerable<Note> Notes { get; set; }

		public IEnumerable<Address> Addresses { get; set; }

		public AddressBookContact ()
		{
		}

		public override string ToString ()
		{
			return string.Format ("[AddressBookContact: Id={0}\n, FirstName={1}\n, LastName={2}\n, DisplayName={3}\n, MiddleName={4}\n, NickName={5}\n," +
				" Prefix={6}\n, Suffix={7}\n, IsAggregate={8}\n, Thumbnail={9}\n, Organizations={10}\n,Phones={11}\n, Emails={12}\n, Notes={13}]", 
				Id, FirstName, LastName, DisplayName, MiddleName, NickName, Prefix, Suffix, IsAggregate, Thumbnail, 
				string.Join("\n\t", Organizations),
				string.Join("\n\t", Phones),
				string.Join("\n\t", Emails),
				string.Join("\n\t", Notes)
			);
		}
	}
}

