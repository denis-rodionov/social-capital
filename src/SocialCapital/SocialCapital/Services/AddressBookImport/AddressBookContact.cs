using System;
using System.Collections.Generic;
using SocialCapital.Data.Model;
using System.Text;
using System.Collections;

namespace SocialCapital.Services.AddressBookImport
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

		public List<Organization> Organizations { get; set; }

		public List<Phone> Phones { get; set; }

		public List<Email> Emails { get; set; }

		public List<Note> Notes { get; set; }

		public List<Address> Addresses { get; set; }

		public long LastUpdatedTimespamp { get; set; }

		#region Experiment fields



		public string HasPhoneNumber { get; set; }

		public string InDefaultDirectory { get; set; }

		public string InVisibleGroup { get; set; }

		public string LookupKey { get; set; }

		public string RawContactId { get; set; }

		public string TimesContacted { get; set; }

		public string PhotoFileId { get; set; }

		public string PhotoId { get; set; }

		public string ThumbnailUri { get; set; }

		public string PhotoUri { get; set; }

		public string Stared { get; set; }

		public string ContactStatusTimestamp { get; set; }

		#endregion

		public AddressBookContact ()
		{
			Phones = new List<Phone> ();
			Emails = new List<Email> ();
			Organizations = new List<Organization> ();
			Addresses = new List<Address> ();
			Notes = new List<Note> ();
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

		string savedDebugString = null;
		public string DebugString {
			get {
				if (savedDebugString == null)
				{
					string res = "";

					res += AppendProperty (Id, "Id");
					res += AppendProperty (FirstName, "FirstName");
					res += AppendProperty (LastName, "LastName");
					res += AppendProperty (DisplayName, "DisplayName");
					res += AppendProperty (MiddleName, "MiddleName");
					res += AppendProperty (NickName, "NickName");
					res += AppendProperty (Prefix, "Prefix");
					res += AppendProperty (Suffix, "Suffix");
					res += AppendProperty (IsAggregate, "IsAggregate");
					res += AppendProperty (Thumbnail, "Thumbnail");
					res += AppendProperty (Organizations, "Organizations");
					res += AppendProperty (Phones, "Phones");
					res += AppendProperty (Emails, "Emails");
					res += AppendProperty (Notes, "Notes");
					res += AppendProperty (Addresses, "Addresses");
					res += AppendProperty (LastUpdatedTimespamp, "LastUpdatedTimespamp");
					res += AppendProperty (HasPhoneNumber, "HasPhoneNumber");
					res += AppendProperty (InDefaultDirectory, "InDefaultDirectory");
					res += AppendProperty (InVisibleGroup, "InVisibleGroup");
					res += AppendProperty (LookupKey, "LookupKey");
					res += AppendProperty (RawContactId, "RawContactId");
					res += AppendProperty (TimesContacted, "TimesContacted");
					res += AppendProperty (PhotoFileId, "PhotoFileId");
					res += AppendProperty (PhotoId, "PhotoId");
					res += AppendProperty (ThumbnailUri, "ThumbnailUri");
					res += AppendProperty (PhotoUri, "PhotoUri");
					res += AppendProperty (Stared, "Stared");
					res += AppendProperty (ContactStatusTimestamp, "ContactStatusTimestamp");

					res += "-------------------------------------------";

					savedDebugString = res;
				}

				return savedDebugString;
			}
		}

		string AppendProperty(object obj, string name)
		{
			if (obj != null)
			{
				string objValue;

				if (obj.GetType ().Name == "Byte[]")
					objValue = "[NOT NULL]";
				else if (obj.GetType ().Name.Contains ("List"))
					objValue = ListToString ((IEnumerable)obj);
				else
					objValue = obj.ToString ();		

				if (objValue == "1")
					return name + "\n";
				else if (objValue == "0" || objValue == "False" || objValue == String.Empty)
					return "";
				else
					return string.Format ("{0} = '{1}'\n", name, objValue);
			}
			else
				return string.Empty;
		}

		string ListToString(IEnumerable list)
		{
			string res = "";
			foreach (var item in list)
			{
				res += "\n\t" + item.ToString ();
			}

			//if (res.Length > 0)
			//	res.Remove (res.Length - 1, 1);

			return res;
		}
	}
}

