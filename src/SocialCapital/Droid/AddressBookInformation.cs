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

[assembly: Dependency(typeof(SocialCapital.Droid.AddressBookInformation))]

namespace SocialCapital.Droid
{
	public class AddressBookInformation : IAddressBookInformation
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

		public async Task<List<AddressBookContact>> GetContacts ()
		{
			var contacts = new List<AddressBookContact> ();

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
				foreach (Contact contact in book.Where(x => x.Phones.Count() != 0).OrderBy(c => c.LastName))
				{
					// Note: on certain android device(Htc for example) it show name in DisplayName Field
					contacts.Add (new AddressBookContact () { 
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
						}),
						Phones = contact.Phones.Select (p => new SocialCapital.Data.Model.Phone () {
							Label = p.Label,
							Number = p.Number,
							Type = ToPhoneType (p.Type)
						}),
						Emails = contact.Emails.Select (e => new SocialCapital.Data.Model.Email () {
							Address = e.Address,
							Label = e.Label,
							Type = ToEmailType (e.Type)
						}),
						Notes = contact.Notes.Select (n => new SocialCapital.Data.Model.Note () {
							Contents = n.Contents,
						})
					});
				}

				Log.GetLogger().Log("Contacts impoted count: {0}", contacts.Count);
			}

			return contacts;
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

