﻿using System;
using System.Collections.Generic;
using SocialCapital.AddressBookImport;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Threading;
using SocialCapital.Data;
using SocialCapital.Data.Model;
using SocialCapital.Data.Synchronization;
using SocialCapital.Common;

namespace SocialCapital.ViewModels
{
	

	/// <summary>
	/// AddressBook state vime model class
	/// </summary>
	public class AddressBookVM : ViewModelBase
	{
		/// <summary>
		/// Percent of retrieving contacts from device to the whole import work
		/// </summary>
		const double RetrievePercent = 0.5;

		DateTime? lastImportTime = null;

		#region Properties

		public ICommand StartImport { get; private set; }

		// TODO: add threadin safety here
		bool isImportRunning = false;
		public bool IsImportRunning {
			get { return isImportRunning; }
			set { SetProperty (ref isImportRunning, value); }
		}

		string status = "";
		public string Status {
			get { return status; }
			set { SetProperty (ref status, value); }
		}

		double progress = 0;
		public double ImportProgress {
			get { return progress; }
			set { SetProperty (ref progress, value); }
		}

		/// <summary>
		/// Imported history
		/// </summary>
		private ObservableCollection<GroupVM<DateTime, ModificationVM>> modificationGroups = 
			new ObservableCollection<GroupVM<DateTime, ModificationVM>>();
		public ObservableCollection<GroupVM<DateTime, ModificationVM>> ModificationGroups 
		{ 
			get { return modificationGroups; }
			private set { SetProperty (ref modificationGroups, value); }
		}

		#endregion

		#region Init

		/// <summary>
		/// Constructor
		/// </summary>
		public AddressBookVM ()
		{
			lastImportTime = new Settings ().LastAddressBookImportTime;
			InitStatus ();
			StartImport = new Command (Import);

			Task.Run (() => InitContactList ());
		}

		void InitContactList()
		{
			var weekAgo = DateTime.Now - TimeSpan.FromDays (7);
			var modifications = Database.GetContactModifications (m => 
				m.Source == SyncSource.AddressBook && m.ModifiedAt > weekAgo);

			var timing = Timing.Start ("InitContactList");

			var collection = modifications
				.GroupBy (
	                 key => key.ModifiedAt,
	                 (key, list) => new GroupVM<DateTime, ModificationVM> () { 
						Group = key, 
						Elements = list.Take (10).Select (m => new ModificationVM (m)).ToList ()
				})
				.OrderByDescending(key => key.Group)
				.ToList ();

			timing.Finish ();

			ModificationGroups = new ObservableCollection<GroupVM<DateTime, ModificationVM>> (collection);
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Background procedure of importing contacts from device address book to database
		/// </summary>
		private async void Import()
		{
			IProgress<ProgressValue> progressReporter = new Progress<ProgressValue> (ReportProgress);

			if (IsImportRunning) {
				Log.GetLogger ().Log ("Import is already running...", LogLevel.Error);
				return;
			}

			IsImportRunning = true;
			Status = AppResources.ImportStatusInProgress;
			Log.GetLogger ().Log ("Starting import task...");

			var imported = await Task.Run<GroupVM<DateTime, ModificationVM>>(() => 
				{
					Log.GetLogger ().Log ("Import started");
					var service = new AddressBookService();
					service.ProgressEvent += (val) => { progressReporter.Report(val); } ;

					service.LoadContacts();
					var res = service.FullUpdate();
					Log.GetLogger().Log("Import finished");

					return res;
				});
			
			ImportFinished (imported);

			Log.GetLogger().Log("Import task exited");
			IsImportRunning = false;
		}

		private void InitStatus()
		{
			if (lastImportTime.HasValue)
				Status = string.Format ("{0}: {1}", AppResources.AddressBookSynchStatus, lastImportTime);
			else
				Status = AppResources.AddressBookNoSynchStatus;
		}

		/// <summary>
		/// Shows the progress on progressBar
		/// </summary>
		/// <param name="value">Value.</param>
		private void ReportProgress(ProgressValue value)
		{
			double res = 0;
			if (value.TotalContacts != 0) {
				if (value.ContactsSync == 0)
					res = (double)value.ContactsRetrieved / value.TotalContacts * RetrievePercent;
				else
					res = (double)value.ContactsSync / value.TotalContacts * (1 - RetrievePercent) + RetrievePercent;
			}

			ImportProgress = res;
		}

		private void ImportFinished(GroupVM<DateTime, ModificationVM> importResult)
		{
			UpdateStatus (importResult.Group, importResult.Count());
			ImportProgress = 0;

			if (importResult.Elements.Any())
				ModificationGroups.Insert (0, importResult);
		}

		void UpdateStatus (DateTime updateTime, int syncCount)
		{
			lastImportTime = updateTime;
			new Settings ().LastAddressBookImportTime = lastImportTime;

			Status = string.Format ("{0}({1}): {2}", AppResources.AddressBookSynchStatus, syncCount, lastImportTime);
		}

		#endregion
	}
}

