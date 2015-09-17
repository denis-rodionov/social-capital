using System;
using SocialCapital.Data.Model;
using System.Collections.ObjectModel;
using SocialCapital.Data;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.IO;
using System.ComponentModel;
using SocialCapital.PhoneServices;
using System.Windows.Input;
using SocialCapital.ViewModels.Commands;

namespace SocialCapital.ViewModels
{
	public class ContactVM : ViewModelBase
	{
		private ImageSource anonimusPhoto = null;

		/// <summary>
		/// Original copy of the contact
		/// </summary>
		public Contact SourceContact { get; private set; }

		#region Commands

		public ICommand CallCommand { get; set; }

		#endregion

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

			SourceContact = contact;

			anonimusPhoto = GetAnonimusPhoto ();

			CallCommand = new MakeCallCommand (Phones);
		}

		#region View Properties

		public string FullName {
			get {return string.Format ("{0}: {1}", SourceContact.Id, SourceContact.DisplayName); }
			set {	
				SourceContact.DisplayName = value;
				OnPropertyChanged ();
			}
		}

		public string WorkPlace {
			get { return SourceContact.WorkPlace;}
			set {
				SourceContact.WorkPlace = value;
				OnPropertyChanged ();
			}
		}

		TagsVM tags = null;
		public TagsVM Tags {
			get {
				if (tags == null)
					tags = new TagsVM (Database.GetContactTags (SourceContact.Id));
				return tags;
			}
			set 
			{ 
				SetProperty (ref tags, value);
				OnPropertyChanged ("TagList");
			}
		}

		public ImageSource PhotoImage {
			get {
				if (SourceContact.Thumbnail == null || SourceContact.Thumbnail.Length == 0)
					return anonimusPhoto;

				var res = ImageSource.FromStream(() => 
					{
						return new MemoryStream(SourceContact.Thumbnail);
					});
				return res;
			}
		}

		public string TagList { 
			get { 
				Tags.PropertyChanged += (sender, e) => { OnPropertyChanged(); };
				return string.Join (",", Tags.Tags.Select(t => t.Name).ToArray ()); 
			} 

		}

		private IEnumerable<Phone> phones = null;
		public IEnumerable<Phone> Phones { 
			get {
				if (phones == null)
					phones = Database.GetContactPhones (SourceContact.Id);
				return phones;
			}
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

		#region implementation

		private ImageSource GetAnonimusPhoto()
		{
			var tempImage = ResourceLoader.LoadFileFromResource ("SocialCapital.Resources.generic_avatar.png");
			var res = ImageSource.FromStream (() =>
				new MemoryStream (tempImage));
			return res;
		}

		#endregion
	}
}

