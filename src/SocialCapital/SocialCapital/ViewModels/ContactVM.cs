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
using Ninject;
using SocialCapital.Data.Managers;
using SocialCapital.Common.FormsMVVM;

namespace SocialCapital.ViewModels
{
	public class ContactVM : ViewModelBase
	{
		private static ImageSource anonimusPhoto = null;

		/// <summary>
		/// Original copy of the contact
		/// </summary>
		public Contact SourceContact { get; private set; }

		#region Init

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

			//timing.Finish (LogLevel.Trace);
		}

		#endregion

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
					tags = new TagsVM (App.Container.Get<ContactTagsManager>().GetContactTags (SourceContact.Id));
				return tags;
			}
			set 
			{ 
				SetProperty (ref tags, value);
			}
		}

		private IEnumerable<Phone> phones = null;
		public IEnumerable<Phone> Phones { 
			get {
				if (phones == null)
					phones = App.Container.Get<PhonesManager>().GetContactPhones (SourceContact.Id);
				return phones;
			}
		}

		private IEnumerable<Email> emails = null;
		public IEnumerable<Email> Emails { 
			get {
				if (emails == null)
					emails = App.Container.Get<EmailManager>().GetContactEmails (SourceContact.Id);
				return emails;
			}
		}

		private IEnumerable<CommunicationHistory> history = null;
		public IEnumerable<CommunicationHistory> History {
			get {
				if (history == null)
					history = App.Container.Get<CommunicationManager>()
						.GetCommunications ((c) => c.ContactId == SourceContact.Id)
						.OrderByDescending(h => h.Time);
				return history;
			}
			set { SetProperty (ref history, value); }
		}

		/// <summary>
		/// Defines of the contact is selected in the list
		/// </summary>
		private bool selected = false;
		public bool Selected {
			get { return selected; }
			set { SetProperty (ref selected, value); }
		}

		private ContactStatus contactStatus;
		public ContactStatus ContactStatus {
			get {
				if (contactStatus == null)
					contactStatus = new ContactStatus (SourceContact, SourceContact.Frequency, SourceContact.LastCommunication);
				return contactStatus;
			}
			set { SetProperty (ref contactStatus, value); }
		}

        #endregion

		#region Commands

		private ICommand callCommand;
		public ICommand CallCommand { 
			get { 
				if (callCommand == null)
				{
					var dialogService = App.Container.Get<IDialogProvider> ();
					callCommand = new MakeCallCommand (SourceContact, () => Phones, dialogService);
					(callCommand as MakeCallCommand).CommandExecuted += OnCommunicationCommandExecuted;
				}
				return callCommand;
			}
		}

		private ICommand smsWriteCommand;
		public ICommand SmSWriteCommand {
			get {
				if (smsWriteCommand == null)
				{
					var dialogService = App.Container.Get<IDialogProvider> ();
					smsWriteCommand = new SmsWriteCommand (SourceContact, () => Phones, dialogService);
					(smsWriteCommand as SmsWriteCommand).CommandExecuted += OnCommunicationCommandExecuted;
				}
				return smsWriteCommand;
			}
		}

		private ICommand writeEmailCommand;
		public ICommand WriteEmailCommand {
			get {
				if (writeEmailCommand == null)
				{
					var dialogService = App.Container.Get<IDialogProvider> ();
					writeEmailCommand = new EmailWriteCommand (SourceContact, () => Emails, dialogService);
					(writeEmailCommand as EmailWriteCommand).CommandExecuted += OnCommunicationCommandExecuted;
				}
				return writeEmailCommand;
			}
		}

		private ICommand logCommunication;
		public ICommand LogCommunication {
			get {
				if (logCommunication == null)
				{
					logCommunication = new LogCommunication (SourceContact);
					(logCommunication as LogCommunication).CommandExecuted += OnCommunicationCommandExecuted;
				}
				return logCommunication;
			}
		}

		public void Save()
		{
			Database.SaveContactInfo (SourceContact);
			App.Container.Get<ContactTagsManager>().SaveContactTags (Tags.Tags, SourceContact.Id);
			Tags = null;
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

		#endregion

		#region Implementation

		private void OnCommunicationCommandExecuted()
		{
			History = null;
			ContactStatus = null;
		}

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

