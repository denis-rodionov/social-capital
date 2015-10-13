using System;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace SocialCapital.Data.Managers
{
	public class LogManager : BaseManager<LogMessage>
	{
		public LogManager (Func<IDataContext> contextFactory) : base(contextFactory)
		{
		}

		public IEnumerable<LogMessage> GetLogs()
		{
			using (var db = CreateContext())
			{
				return db.Connection.Table<LogMessage>().ToList();
			}
		}

		public void SaveLog(LogMessage log)
		{
			using (var db = CreateContext())
			{
				db.Connection.Insert (log);
				if (log.Id == 0)
					throw new DataManagerException ("Log is not saved to database\n" + log.Message);
			}
		}

		protected override void InnerRefreshCache (IDataContext db)
		{
			// no caching fot logs
		}
	}
}

