using System;
using SocialCapital.Data.Model;
using Ninject;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace SocialCapital.Data
{
	public class GroupsManager
	{
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

				db.Connection.Insert (new Group () {
					Name = AppResources.RelativesGroupName,
					Description = AppResources.RelativesGroupDescription,
					FrequencyId = fm.GetFrequency(db, PeriodValues.Month, 1).Id,
					IsArchive = false
				});

				db.Connection.Insert (new Group () {
					Name = AppResources.ColleguesGroupName,
					Description = AppResources.ColleguesGroupDescription,
					FrequencyId = fm.GetFrequency(db, PeriodValues.Never, 0).Id,
					IsArchive = false
				});

				db.Connection.Insert (new Group () {
					Name = AppResources.UsefulGroupName,
					Description = AppResources.UsefulGroupDescription,
					FrequencyId = fm.GetFrequency(db, PeriodValues.Month, 1).Id,
					IsArchive = false
				});

				db.Connection.Insert (new Group () {
					Name = AppResources.ArchiveGroupName,
					Description = AppResources.ArchiveGroupDescription,
					FrequencyId = fm.GetFrequency(db, PeriodValues.Never, 0).Id,
					IsArchive = true
				});
			}
		}

		public Group GetGroup(int groupId)
		{
			using (var db = new DataContext ())
			{
				var res = db.Connection.Table<Group> ().SingleOrDefault (g => g.Id == groupId);

				if (res == null)
					throw new Exception ("No group with such Id");

				return res;
			}
		}

		public IEnumerable<Group> GetAllGroups(Expression<Func<Group, bool>> whereClause)
		{
			using (var db = new DataContext ())
			{
				return db.Connection.Table<Group> ().Where (whereClause).ToList ();
			}
		}

		#region Save actions

		public void UpdateGroupData(Group group)
		{
			using (var db = new DataContext ())
			{
				db.Connection.Update (group);
			}
		}

		public void UpdateFrequency(Group group)
		{
			var frequencyManager = App.Container.Get<FrequencyManager> ();

			using (var db = new DataContext ())
			{
				//db.Connection.BeginTransaction ();
				var frequency = frequencyManager.GetFrequency (db, group.Frequency.Period, group.Frequency.Count);
				if (frequency.Id != group.FrequencyId)
					db.Connection.Execute ("UPDATE [Group] SET FrequencyId=? WHERE Id=?", frequency.Id, group.Id);
				//db.Connection.Commit ();
			}
		}

		#endregion
	}
}

