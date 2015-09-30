using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Contacts;
using SocialCapital.AddressBookImport;
using System.Threading.Tasks;
using Android.Graphics;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;

//[assembly: Dependency(typeof(SocialCapital.Droid.Services.AddressBookInformation))]

namespace SocialCapital.Droid.Services
{
	public class AddressBookInformation : BaseAddressBookService, IAddressBookInformation
	{
		

		/// <summary>
		/// The book.
		/// </summary>
		private AddressBook book = null;





		/// <summary>
		/// Initializes a new instance of the <see cref="AddressBookInformation"/> class.
		/// </summary>
		public AddressBookInformation()
		{
			this.book = new AddressBook(Forms.Context.ApplicationContext);
		}

		/// <summary>
		/// Gets the contacts by means of Xamarin.Mobile
		/// </summary>
		/// <returns>The contacts.</returns>
		public IEnumerable<AddressBookContact> GetContacts ()
		{
			IEnumerable<AddressBookContact> contacts;

			CountRetrieved = 0;

			// Observation:
			// On device RequestPermission() returns false sometimes so you can use  this.book.RequestPermission().Result (remove await)
			//var permissionResult = await this.book.RequestPermission();
			if (true)
			{
				if (!this.book.Any())
				{
					Log.GetLogger().Log("No contacts found");
				}

				Log.GetLogger().Log("Start geting contacts...");
				//var raw = book.Where (GetFilter ()).Take (700).ToList ();

				var countLimit = book.Count ();
				RaiseCountCalculated (countLimit);

				contacts = book.Take (countLimit).Select (bc => ConvertToContact (bc)).ToList ();

				//foreach (Contact contact in bookContacts)
				//{
					// Note: on certain android device(Htc for example) it show name in DisplayName Field
				//	contacts.Add( ConvertToContact (contact));
				//}

				Log.GetLogger().Log("Contacts impoted count: {0}", contacts.Count());
			}

			return contacts;
		}





		private Expression<Func<Contact, bool>> GetFilter()
		{
			return x => x.Phones.Count () != 0;
		}

		private AddressBookContact ConvertToContact (Contact contact)
		{
			var res = new AddressBookContact () {
				Id = contact.Id,
				FirstName = contact.FirstName,
				LastName = contact.LastName,
				MiddleName = contact.MiddleName,
				DisplayName = contact.DisplayName,
				NickName = contact.Nickname,
				Thumbnail = GetImageArray (contact.GetThumbnail ()),
				IsAggregate = contact.IsAggregate,
				Prefix = contact.Prefix,
				Suffix = contact.Suffix,
				Organizations = contact.Organizations.Select (o => new SocialCapital.Data.Model.Organization () {
					ContactTitle = o.ContactTitle,
					Label = o.Label,
					Name = o.Name
				}).ToList(),
				Phones = contact.Phones.Select (p => new SocialCapital.Data.Model.Phone () {
					Label = p.Label,
					Number = p.Number,
					Type = ToPhoneType (p.Type)
				}).ToList(),
				Emails = contact.Emails.Select (e => new SocialCapital.Data.Model.Email () {
					Address = e.Address,
					Label = e.Label,
					Type = ToEmailType (e.Type)
				}).ToList(),
				Notes = contact.Notes.Select (n => new SocialCapital.Data.Model.Note () {
					Contents = n.Contents,
				}).ToList(),
				Addresses = contact.Addresses.Select(a => new SocialCapital.Data.Model.Address() {
					Country = a.Country,
					City = a.City,
					Region = a.Region,
					PostalCode = a.PostalCode,
					StreetAddress = a.StreetAddress,
					Label = a.Label
				}).ToList()
			};

			RaiseContactRetrieved (CountRetrieved++);

			return res;
		}



		private SocialCapital.Data.Model.EmailType ToEmailType(EmailType type)
		{
			switch (type) {
				case EmailType.Home:
					return SocialCapital.Data.Model.EmailType.Home;
				case EmailType.Other:
					return SocialCapital.Data.Model.EmailType.Other;
				case EmailType.Work:
					return SocialCapital.Data.Model.EmailType.Work;
				default:
					return SocialCapital.Data.Model.EmailType.Other;
			}
		}

		private SocialCapital.Data.Model.PhoneType ToPhoneType(PhoneType type)
		{
			switch (type) {
				case PhoneType.Home:
					return SocialCapital.Data.Model.PhoneType.Home;
				case PhoneType.HomeFax:
					return SocialCapital.Data.Model.PhoneType.HomeFax;
				case PhoneType.Mobile:
					return SocialCapital.Data.Model.PhoneType.Mobile;
				case PhoneType.Other:
					return SocialCapital.Data.Model.PhoneType.Other;
				case PhoneType.Pager:
					return SocialCapital.Data.Model.PhoneType.Pager;
				case PhoneType.Work:
					return SocialCapital.Data.Model.PhoneType.Work;
				case PhoneType.WorkFax:
					return SocialCapital.Data.Model.PhoneType.WorkFax;			
				default:
					return SocialCapital.Data.Model.PhoneType.Other;
			}
		}

		private byte[] GetImageArray(Bitmap bitmap)
		{
			if (bitmap == null)
				return null;
			
			using (var stream = new MemoryStream ()) {
				bitmap.Compress (Bitmap.CompressFormat.Png, 0, stream);
				return stream.ToArray ();
			}
		}
	}
}

