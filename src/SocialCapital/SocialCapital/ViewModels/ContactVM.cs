using System;
using SocialCapital.Data.Model;
using System.Collections.ObjectModel;
using SocialCapital.Data;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.IO;

namespace SocialCapital.ViewModels
{
	public class ContactVM 
	{
		/// <summary>
		/// Manager class for accessing database
		/// </summary>
		public ContactManager Database { get; private set; }

		/// <summary>
		/// Original copy of the contact
		/// </summary>
		public Contact SourceContact { get; private set; }

		/// <summary>
		/// VM Constructor
		/// </summary>
		/// <param name="contact">Contact model</param>
		public ContactVM (Contact contact)
		{
			if (contact == null)
				throw new ArgumentException ();

			if (contact.Id < 1)
				throw new ArgumentException ();

			Database = new ContactManager ();

			SourceContact = contact;
		}

		#region View Properties

		public string FullName {
			get {
				return SourceContact.FullName;
			}
			set {
				SourceContact.FullName = value;
			}
		}

		public string WorkPlace {
			get {
				return SourceContact.WorkPlace;
			}
			set {
				SourceContact.WorkPlace = value;
			}
		}

		IEnumerable<Tag> tags = null;
		public IEnumerable<Tag> Tags {
			get {
				if (tags == null)
					tags = Database.GetContactTags (SourceContact.Id);
				return tags;
			}
		}

		public ImageSource PhotoImage {
			get {
				// Log.GetLogger ().Log ("=== PhotoImage request ===");
				if (SourceContact.Photo == null || SourceContact.Photo.Length == 0)
					return null;

				var res = ImageSource.FromStream(() => 
					{
						return new MemoryStream(SourceContact.Photo);
					});
				return res;
			}
		}

		#endregion

		public void Save()
		{
			Database.SaveContactInfo (SourceContact);
			Database.SaveContactTags (Tags, SourceContact.Id);
		}

		public string TagList { get { return string.Join (",", Tags.Select(t => t.Name).ToArray ()); } }
	}
}

