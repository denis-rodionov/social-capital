using System;
using SocialCapital.Data.Model;
using Ninject;
using Ninject.Parameters;
using System.Linq;
using System.Collections.Generic;

namespace SocialCapital.Data.Managers
{
	public class FrequencyManager : BaseManager<Frequency>
	{
		public FrequencyManager ()
		{
		}

		public void InitFrequencies()
		{
			using (var db = new DataContext ())
			{
				InitFrequencies (db);
			}
		}

		public void InitFrequencies(DataContext db)
		{
			if (Count (db) != 0)
				throw new Exception ("Frequencies already filled");

			var stdFrequencies = new List<Frequency> ();

			// once a year
			stdFrequencies.Add (new Frequency () {
				Period = PeriodValues.Year,
				Count = 1
			});

			// twise a year
			stdFrequencies.Add (new Frequency () {
				Period = PeriodValues.Year,
				Count = 2
			});

			// once a quater
			stdFrequencies.Add (new Frequency () {
				Period = PeriodValues.Year,
				Count = 4
			});

			// once a month
			stdFrequencies.Add (new Frequency () {
				Period = PeriodValues.Month,
				Count = 1
			});

			InsertAll (stdFrequencies);
		}

		public Frequency GetFrequency(DataContext db, PeriodValues period, int count)
		{
			var res = Find(f => f.Period == period && f.Count == count, db);

			if (res == null)
			{
				res = new Frequency () { Period = period, Count = count };
				Insert (res, db);
			}

			return res;
		}

		public Frequency GetFrequency(int frequencyId)
		{
			return Get (frequencyId);
		}
	}
}

