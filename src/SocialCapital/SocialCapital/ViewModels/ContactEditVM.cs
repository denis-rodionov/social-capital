using System;
using Xamarin.Forms;
using SocialCapital.Data.Model;
using System.Collections;
using System.Collections.Generic;
using SocialCapital.Data.Managers;

namespace SocialCapital.ViewModels
{
	public class ContactEditVM : ViewModelBase
	{
		private ContactManager contactManager;
		private ContactTagsManager tagManager;
		private Contact sourceContact;

		public ContactEditVM (Contact contactModel, IEnumerable<Tag> tags, ContactManager contactManager, ContactTagsManager tagManager)
		{
			this.contactManager = contactManager;
			this.tagManager = tagManager;
			this.sourceContact = contactModel;

			this.fullName = contactModel.DisplayName;
			this.workPlace = contactModel.WorkPlace;
			this.tags = new TagsVM (tags);
			this.frequent = contactModel.Frequent;
			this.birthdate = contactModel.Birthdate;
		}

		#region Properties

		string fullName;
		public string FullName {
			get { return fullName; }
			set { SetProperty (ref fullName, value); }
		}

		string workPlace;
		public string WorkPlace {
			get { return workPlace; }
			set { SetProperty (ref workPlace, value); }
		}

		TagsVM tags = null;
		public TagsVM Tags {
			get { return tags; }
			set { SetProperty (ref tags, value); }
		}

		DateTime? birthdate;
		public DateTime Birthdate {
			get { return birthdate.HasValue ? birthdate.Value : DateTime.Now.AddYears(-30); }
			set { 
				birthdate = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// Service field need for DatePicker
		/// </summary>
		private bool? hasBirthdate;
		public bool HasBirthdate { 
			get { 
				if (!hasBirthdate.HasValue)
					hasBirthdate = birthdate.HasValue;
				return  hasBirthdate.Value;
			} 
			set { SetProperty(ref hasBirthdate, value); }
		}

		private bool frequent;
		public bool Frequent {
			get { return frequent; }
			set { SetProperty (ref frequent, value); }
		}

		#endregion

		#region Actions

		public void Save()
		{
			var resContact = ToModel ();
			contactManager.SaveContactInfo (resContact);

			tagManager.SaveContactTags (Tags.Tags, resContact.Id);
			Tags = null;
		}

		private Contact ToModel()
		{
			var source = sourceContact;

			if (HasBirthdate)
				source.Birthdate = Birthdate;
			else
				source.Birthdate = null;

			source.DisplayName = FullName;
			source.WorkPlace = WorkPlace;
			source.Frequent = Frequent;

			return source;
		}

		#endregion
	}
}

