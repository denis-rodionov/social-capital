using System;
using SocialCapital.Data.Model;
using Ninject;
using Ninject.Parameters;
using System.Linq;

namespace SocialCapital.Data
{
	public class FrequencyManager : BaseDataManager
	{
		public FrequencyManager (IDataContext dataContext = null)
		{
		}

		public void InitFrequencies()
		{
			using (var db = new DataContext ())
			{
				InitFrequencies (db);
			}
		}

		public void InitFrequencies(IDataContext db)
		{
			if (db.Connection.Table<Frequency> ().Count () != 0)
				throw new Exception ("Frequencies already filled");

			// once a year
			db.Connection.Insert (new Frequency () {
				Period = PeriodValues.Year,
				Count = 1
			});

			// twise a year
			db.Connection.Insert (new Frequency () {
				Period = PeriodValues.Year,
				Count = 2
			});

			// once a quater
			db.Connection.Insert (new Frequency () {
				Period = PeriodValues.Year,
				Count = 4
			});

			// once a month
			db.Connection.Insert (new Frequency () {
				Period = PeriodValues.Month,
				Count = 1
			});
		}

		public Frequency GetFrequency(IDataContext db, PeriodValues period, int count)
		{
			var res = db.Connection.Table<Frequency> ().SingleOrDefault (f => f.Period == period && f.Count == count);

			if (res == null)
			{
				res = new Frequency () { Period = period, Count = count };
				db.Connection.Insert (res);
				CheckId (res);
			}

			return res;
		}

		public Frequency GetFrequency(int frequencyId)
		{
			using (var db = new DataContext ())
			{
				var res = db.Connection.Table<Frequency> ().SingleOrDefault (f => f.Id == frequencyId);
				if (res == null)
					throw new Exception ("No Frequency object in database with such id = " + frequencyId);
				return res;
			}
		}
	}
}

