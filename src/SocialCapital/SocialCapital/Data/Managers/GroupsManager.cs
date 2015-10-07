using System;
using SocialCapital.Data.Model;
using Ninject;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using SocialCapital.Data.Managers;

namespace SocialCapital.Data.Managers
{
	public class GroupsManager : BaseManager<Group>
	{
		const PeriodValues DefaultPeriod = PeriodValues.Year;
		const int DefaultPeriodCount = 2;

		public GroupsManager ()
		{
		}

		/// <summary>
		///  creates typical groups
		/// </summary>
		public void Init()
		{
			var fm = App.Container.Get<FrequencyManager> ();

			using (var db = new DataContext ())
			{		
				fm.InitFrequencies (db);

				var defaultGroups = new List<Group> ();

				defaultGroups.Add (new Group () {
					Name = AppResources.RelativesGroupName,
					Description = AppResources.RelativesGroupDescription,
					FrequencyId = fm.GetFrequency(db, PeriodValues.Month, 1).Id,
					IsArchive = false
				});

				defaultGroups.Add (new Group () {
					Name = AppResources.ColleguesGroupName,
					Description = AppResources.ColleguesGroupDescription,
					FrequencyId = fm.GetFrequency(db, PeriodValues.Never, 0).Id,
					IsArchive = false
				});

				defaultGroups.Add (new Group () {
					Name = AppResources.UsefulGroupName,
					Description = AppResources.UsefulGroupDescription,
					FrequencyId = fm.GetFrequency(db, PeriodValues.Month, 1).Id,
					IsArchive = false
				});

				defaultGroups.Add (new Group () {
					Name = AppResources.ArchiveGroupName,
					Description = AppResources.ArchiveGroupDescription,
					FrequencyId = fm.GetFrequency(db, PeriodValues.Never, 0).Id,
					IsArchive = true
				});

				InsertAll (defaultGroups, db);
			}
		}

		public Group GetGroup(int groupId)
		{
			return Get (groupId);
		}

		public IEnumerable<Group> GetAllGroups(Func<Group, bool> whereClause)
		{
			return GetList (whereClause);
		}

		#region Save actions

		public void UpdateGroupData(Group group)
		{
			Update (group);
		}

		public void UpdateFrequency(Group group)
		{
			var frequencyManager = App.Container.Get<FrequencyManager> ();

			using (var db = new DataContext ())
			{
				db.Connection.BeginTransaction ();

				var frequency = frequencyManager.GetFrequency (db, group.Frequency.Period, group.Frequency.Count);
				if (frequency.Id != group.FrequencyId)
					db.Connection.Execute ("UPDATE [Group] SET FrequencyId=? WHERE Id=?", frequency.Id, group.Id);

				ItemUpdated (group);
				
				db.Connection.Commit ();
			}
		}

		public Group CreateNewGroup()
		{
			var frequencyManager = App.Container.Get<FrequencyManager> ();
			Group newGroup;

			using (var db = new DataContext ())
			{
				var frequency = frequencyManager.GetFrequency (db, DefaultPeriod, DefaultPeriodCount);

				newGroup = new Group () {
					Name = null,
					Description = null,
					FrequencyId = frequency.Id,
					IsArchive = false
				};

				Insert (newGroup);
			}

			return newGroup;
		}

		public void DeleteGroup(Group group)
		{
			Delete (group);
		}

		#endregion
	}
}

