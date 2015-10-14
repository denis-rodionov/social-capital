using System;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using SocialCapital.Data;
using Ninject;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using SocialCapital.Data.Managers;

namespace SocialCapital.ViewModels
{
	public class ContactGroupVM : ViewModelBase
	{
		public Group SourceGroup { get; private set; }

		public ContactGroupVM (Group gr)
		{
			SourceGroup = gr;
			UpdateGroupCommand = new Command (() => { 
				EditMode = false;
				UpdateGroup ();
			});

			EditModeCommand = new Command (() => {  
				EditMode = !EditMode; 
			});

			DeleteCommand = new Command (page => DeleteGroup((Page)page));
		}

		#region Propertied

		public ICommand UpdateGroupCommand { get; set; }
		public ICommand EditModeCommand { get; set; }
		public ICommand DeleteCommand { get; set; }

		private bool editMode = false;
		public bool EditMode {
			get { return editMode; }
			set { SetProperty (ref editMode, value); }
		}

		public string Name {
			get { return SourceGroup.Name; }
			set { 
				SourceGroup.Name = value; 
				OnPropertyChanged ();
			}
		}

		public IEnumerable<Contact> AssignedContacts {
			get { return SourceGroup.AssignedContacts; }
			set { 
				SourceGroup.AssignedContacts = null;
				OnPropertyChanged ();
			}
		}

		public IEnumerable<Frequency> FrequencyList { 
			get {
				return App.Container.Get<FrequencyManager> ().GetAllFrequencies ();
			}
		}

		public Frequency Frequency {
			get { return SourceGroup.Frequency; }
			set { 
				if (SourceGroup.Frequency != value)
				{
					SourceGroup.Frequency = value;
					OnPropertyChanged ();
					UpdateFrequency ();
				}
			}
		}

		public string Description {
			get { return SourceGroup.Description; }
			set {
				if (SourceGroup.Description != value)
				{
					SourceGroup.Description = value;
					OnPropertyChanged ();
					UpdateGroup ();
				}
			}
		}

		#endregion

		#region Actions

		public void Assign(IEnumerable<Contact> contacts)
		{
			App.Container.Get<ContactManager> ().AssignToGroup (contacts, SourceGroup.Id);
			AssignedContacts = null;
		}

		public Task UpdateGroup()
		{
			var db = App.Container.Get<GroupsManager> ();
			return Task.Run (() => db.UpdateGroupData (SourceGroup));
		}

		public Task UpdateFrequency()
		{
			var db = App.Container.Get<GroupsManager> ();
			return Task.Run (() => db.UpdateFrequency (SourceGroup));
		}

		public async void DeleteGroup(Page page)
		{
			var yes = await page.DisplayAlert (AppResources.Warning, AppResources.AreYouSureToDeleteGroup, AppResources.Yes, AppResources.No);

			if (yes)
			{
				var db = App.Container.Get<GroupsManager> ();

				await Task.Run (() => db.DeleteGroup (SourceGroup));

				page.Navigation.PopAsync ();
			} 
		}

		#endregion
	}
}

