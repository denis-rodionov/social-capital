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
		public FrequencyManager (Func<IDataContext> contextFactory) : base(contextFactory)
		{
		}

		public void InitFrequencies()
		{
			using (var db = CreateContext())
			{
				InitFrequencies (db);
			}
		}

		public void InitFrequencies(IDataContext db)
		{
			if (Count (db) != 0)
				throw new Exception ("Frequencies already filled");

			var stdFrequencies = new List<Frequency> ();

			// once a year
			stdFrequencies.Add (new Frequency () {
				Name = AppResources.OnceAYear,
				Period = 365,
				Never = false
			});

			// twise a year
			stdFrequencies.Add (new Frequency () {
				Name = AppResources.TwiceAYear,
				Period = 182,
				Never = false
			});

			// once a quater
			stdFrequencies.Add (new Frequency () {
				Name = AppResources.OnceAQuarter,
				Period = 91,
				Never = false
			});

			// once a month
			stdFrequencies.Add (new Frequency () {
				Name = AppResources.OnceAMonth,
				Period = 30,
				Never = false
			});

			// never
			stdFrequencies.Add (new Frequency () {
				Name = AppResources.Never,
				Period = 0,
				Never = true
			});

			// twice a month
			stdFrequencies.Add (new Frequency () {
				Name = AppResources.TwiceAMonth,
				Period = 15,
			});

			InsertAll (stdFrequencies);
		}

		public IEnumerable<Frequency> GetAllFrequencies()
		{
			return GetList (f => true).OrderBy (f => f.Period);
		}

		public Frequency GetFrequency(string name, IDataContext db = null)
		{
			var res = Find(f => f.Name == name, db);

			if (res == null)
			{
				throw new DataManagerException (string.Format ("No frequency with name = '{0}' in database"));
			}

			return res;
		}

		public Frequency GetFrequency(int frequencyId)
		{
			return Get (frequencyId);
		}

		public void AddFrequency(string name, double period, IDataContext db = null)
		{
			var existing = Find (f => f.Name == name, db);

			if (existing == null)
				Insert (new Frequency () { Name = name, Period = period }, db);
		}
	}
}

