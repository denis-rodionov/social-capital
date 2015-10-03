using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using SocialCapital.Data.Model.Enums;

namespace SocialCapital.Data.Managers
{
	public abstract class BaseManager<Type> where Type : class, IHaveId, IEquatable<Type>
	{
		public List<Type> Cache { get; set; }

		public BaseManager ()
		{
		}

		public void CheckId(IHaveId dbObject)
		{
			if (dbObject.Id == 0)
				throw new Exception(string.Format("Database object {0} has incorrect id == 0", dbObject));
		}

		public void RefreshCache(DataContext db)
		{
			Cache = db.Connection.Table<Type> ().ToList ();
			Log.GetLogger ().Log ("Cache updated for " + typeof(Type), LogLevel.Trace);
		}

		/// <summary>
		/// Update filtered list: take as argument result list: 
		/// turn filtered list to actual
		/// </summary>
		public void UpdateList(IEnumerable<Type> actualList, DataContext db,
			Expression<Func<Type, bool>> whereClause)
		{
			var existingList = db.Connection.Table<Type> ().Where (whereClause).ToList();
			var newList = actualList.Except (existingList).ToList();
			var deleteList = existingList.Except (actualList).ToList();

			foreach (var item in newList) 
				Insert (item, db);

			foreach (var item in deleteList)
				Delete (item, db);
		}

		public void Insert(Type item, DataContext db)
		{
			db.Connection.Insert (item);
			ItemInserted (item);
		}

		public void ItemInserted(Type item)
		{
			if (Cache != null)
			{
				Cache.Add (item);
			}
		}

		public void Delete(Type item, DataContext db)
		{
			db.Connection.Delete(item);
			ItemDeleted (item);
		}

		public void ItemDeleted(Type item)
		{
			if (Cache != null)
			{
				var exist = Cache.SingleOrDefault (c => c.Id == item.Id);
				if (exist != null)
					Cache.Remove (exist);
			}
		}

		public void Update(Type item, DataContext db)
		{
			db.Connection.Update (item);
			ItemUpdated (item);
		}

		public void ItemUpdated(Type item)
		{
			if (Cache != null)
			{
				var existing = Cache.SingleOrDefault (t => t.Id == item.Id);
				if (existing != null)
					Cache.Remove (existing);
				Cache.Add (existing);
			}
		}

		public Type Get(int Id, DataContext db)
		{
			if (Cache == null)
				RefreshCache (db);

			Type res = Cache.SingleOrDefault (c => c.Id == Id);
			if (res == null)
			{
				RefreshCache (db);
				res = Cache.SingleOrDefault (c => c.Id == Id);
				if (res == null)
					throw new DataManagerException (string.Format ("Cannot item '{0}' with Id={1}", typeof(Type), Id));
			}
			else
				return res;
			
			return res;
		}

		public List<Type> GetList(Func<Type, bool> whereClause, DataContext db)
		{
			if (Cache == null)
				RefreshCache (db);
			return Cache.Where (whereClause).ToList ();
		}

		public void ClearCache()
		{
			Cache = null;
		}
	}
}

