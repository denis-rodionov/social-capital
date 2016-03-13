using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using SocialCapital.Data.Model.Enums;
using SocialCapital.Common;

namespace SocialCapital.Data.Managers
{
	public abstract class BaseManager<Type> : ICachable
		where Type : class, IHaveId, IEquatable<Type>
	{
		private Func<IDataContext> contextFactory;

		public BaseManager (Func<IDataContext> contextFactory)
		{
			this.contextFactory = contextFactory;
		}

		public IDataContext CreateContext()
		{
			var db = contextFactory();
			Log.GetLogger().Log(string.Format("DataContext for type [{0}]", typeof(Type)), LogLevel.Trace);
			return db;
		}

		#region Cache

		public List<Type> Cache { get; set; }

		public void RefreshCache(IDataContext db = null)
		{
			var timing = Timing.Start ("Cache updated for " + typeof(Type));

			if (db == null)
				using (var innerDb = CreateContext ())
					InnerRefreshCache (innerDb);
			else
				InnerRefreshCache (db);

			timing.Finish (LogLevel.Debug);
		}

		protected virtual void InnerRefreshCache(IDataContext db)
		{
			Cache = db.Connection.Table<Type> ().ToList ();
		}

		public void ClearCache()
		{
			Cache = null;
		}

        #endregion

		#region Insert

		protected Type Insert(Type item, IDataContext db = null)
		{
			if (item == null)
				throw new ArgumentException ("Item to insert is null");

			if (db == null)
				using (var innerDb = CreateContext())
					innerDb.Connection.Insert (item);
			else
				db.Connection.Insert (item);

			CheckId (item);			
			ItemInserted (item);

			return item;
		}

		public void ItemInserted(Type item)
		{
			if (Cache != null)
			{
				Cache.Add (item);
			}
		}

		public void CheckId(IHaveId dbObject)
		{
			if (dbObject.Id == 0)
				throw new Exception(string.Format("Database object {0} has incorrect id == 0", dbObject));
		}

		protected void InsertAll(IEnumerable<Type> items, IDataContext db = null)
		{
			if (db == null)
				using (var innerDb = CreateContext())
					innerDb.Connection.InsertAll (items);
			else
				db.Connection.InsertAll (items);

			foreach (var item in items)
				ItemInserted (item);
		}

		#endregion

		#region Delete

		protected void Delete(Type item, IDataContext db = null)
		{
			if (db == null)
				using (var innerDb = CreateContext())
					innerDb.Connection.Delete (item);
			else
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

		#endregion

		#region Update

		protected void Update(Type item, IDataContext db = null)
		{
			if (db == null)
			{
				using (var innerDB = CreateContext())
				{
					InnerUpdate (item, innerDB);
				}
			} else
				InnerUpdate (item, db);
				
			ItemUpdated (item);
		}

		protected void InnerUpdate(Type item, IDataContext db)
		{
			db.Connection.Update (item);
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

		/// <summary>
		/// Update filtered list: take as argument result list: 
		/// turn filtered list to actual
		/// </summary>
		public void UpdateList(IEnumerable<Type> actualList, Func<Type, bool> whereClause, IDataContext db = null)
		{
			var existingList = GetList (whereClause, db);

			var newList = actualList.Except (existingList).ToList();
			var deleteList = existingList.Except (actualList).ToList();

			foreach (var item in newList) 
				Insert (item, db);

			foreach (var item in deleteList)
				Delete (item, db);
		}

		#endregion

		#region Get

		protected Type Get(int Id, IDataContext db = null)
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

		protected Type Find(Func<Type, bool> whereClause, IDataContext db = null)
		{
			if (Cache == null)
				RefreshCache (db);

			var res = Cache.Where (whereClause);

			if (res.Count() > 1)
				throw new DataManagerException (string.Format ("More than one element found of type {0}", typeof(Type)));
			else 
				return res.SingleOrDefault ();		
		}

		protected List<Type> GetList(Func<Type, bool> whereClause, IDataContext db = null)
		{
			if (Cache == null)
				RefreshCache (db);
			return Cache.Where (whereClause).ToList ();
		}

		#endregion

		#region Count

		public int Count(IDataContext db = null)
		{
			if (Cache == null)
				RefreshCache (db);

			return Cache.Count;
		}

		#endregion
	}
}

