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
using SocialCapital.Common;

namespace SocialCapital.ViewModels
{
	public class ContactVM : ViewModelBase
	{
		private static ImageSource anonimusPhoto = null;

		/// <summary>
		/// Original copy of the contact
		/// </summary>
		public Contact SourceContact { get; private set; }

		#region Commands

		public ICommand CallCommand { get; set; }

		public ICommand SmSWriteCommand { get; set; }

		public ICommand WriteEmailCommand { get; set; }

		#endregion

		/// <summary>
		/// VM Constructor
		/// </summary>
		/// <param name="contact">Contact model</param>
		public ContactVM (Contact contact)
		{
			//var timing = Timing.Start ("ContactVM.Constructor");

			if (contact == null)
				throw new ArgumentException ();

			if (contact.Id < 1)
				throw new ArgumentException ();

			SourceContact = contact;

			if (anonimusPhoto == null)
				anonimusPhoto = GetAnonimusPhoto ();

			InitCommands ();

			//timing.Finish (LogLevel.Trace);
		}

		void InitCommands ()
		{
			CallCommand = new MakeCallCommand (SourceContact, () => Phones);
			(CallCommand as MakeCallCommand).CommandExecuted += () => {
				History = null;
			};
				
			SmSWriteCommand = new SmsWriteCommand (SourceContact, () => Phones);
			(SmSWriteCommand as SmsWriteCommand).CommandExecuted += () => {
				History = null;
			};
				
			WriteEmailCommand = new EmailWriteCommand (SourceContact, () => Emails);
			(WriteEmailCommand as EmailWriteCommand).CommandExecuted += () => {
				History = null;
			};
		}

		#region View Properties

		public string FullName {
			get {return SourceContact.DisplayName; }
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

		private IEnumerable<Phone> phones = null;
		public IEnumerable<Phone> Phones { 
			get {
				if (phones == null)
					phones = Database.GetContactPhones (SourceContact.Id);
				return phones;
			}
		}

		private IEnumerable<Email> emails = null;
		public IEnumerable<Email> Emails { 
			get {
				if (emails == null)
					emails = Database.GetContactEmails (SourceContact.Id);
				return emails;
			}
		}

		private IEnumerable<CommunicationHistory> history = null;
		public IEnumerable<CommunicationHistory> History {
			get {
				if (history == null)
					history = Database.GetContactCommunications ((c) => c.ContactId == SourceContact.Id);
				return history;
			}
			set { SetProperty (ref history, value); }
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
			return ImageSource.FromFile ("avatar_placeholder.gif");
		}

		/// <summary>
		/// Gets the image from resource.
		/// </summary>
		/// <returns>The image from resource.</returns>
		/// <param name="name">Name of resource. For instance: SocialCapital.Resources.generic_avatar.png</param>
		private ImageSource GetImageFromResource(string name)
		{
			var tempImage = ResourceLoader.LoadFileFromResource (name);
			var res = ImageSource.FromStream (() =>
				new MemoryStream (tempImage));
			return res;
		}

		#endregion
	}
}

