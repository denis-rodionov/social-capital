﻿using System;
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

			InsertAll (stdFrequencies);
		}

		public IEnumerable<Frequency> GetAllFrequencies()
		{
			return GetList (f => true);
		}

		public Frequency GetFrequency(string name, DataContext db = null)
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
	}
}

