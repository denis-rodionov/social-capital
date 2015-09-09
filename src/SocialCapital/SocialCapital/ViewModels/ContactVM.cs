using System;
using SocialCapital.Data.Model;
using System.Collections.ObjectModel;
using SocialCapital.Data;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.IO;
using System.ComponentModel;

namespace SocialCapital.ViewModels
{
	public class ContactVM : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

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
				RaicePropertyChenged ("FullName");
			}
		}

		public string WorkPlace {
			get {
				return SourceContact.WorkPlace;
			}
			set {
				SourceContact.WorkPlace = value;
				RaicePropertyChenged ("WorkPlace");
			}
		}

		TagsVM tags = null;
		public TagsVM Tags {
			get {
				//if (tags == null)
				//	tags = new TagsVM (Database.GetContactTags (SourceContact.Id));
				return tags;
			}
			set 
			{ 
				tags = value; 
				RaicePropertyChenged ("Tags");
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

		public string AbContact { get { return SourceContact.AbContact.ToString (); } }

		public string TagList { get { return string.Join (",", Tags.Tags.Select(t => t.Name).ToArray ()); } }

		#endregion

		#region NotifyPropertyChanged 

		private void RaicePropertyChenged(string property)
		{
			var p = PropertyChanged;

			if (p != null)
				p (this, new PropertyChangedEventArgs (property));
		}

		#endregion

		public void Save()
		{
			Database.SaveContactInfo (SourceContact);
			Database.SaveContactTags (Tags.Tags, SourceContact.Id);
		}

		public void Reload()
		{
			SourceContact = Database.GetContact (SourceContact.Id);
			Tags = null;
		}

		public override string ToString ()
		{
			return string.Format ("[ContactVM: {0}]", SourceContact);
		}
	}
}

