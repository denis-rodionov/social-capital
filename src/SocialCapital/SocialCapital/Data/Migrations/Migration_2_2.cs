using System;
using SocialCapital.Data.Managers;
using Ninject;
using System.Linq;
using System.Threading;

namespace SocialCapital.Data.Migrations
{
	public class Migration_2_2 : IMigration
	{
		public Migration_2_2 ()
		{
			
		}

		#region IMigration implementation

		public void Migrate (IDataContext db)
		{
			var frequencyManager = App.Container.Get<FrequencyManager> ();

			frequencyManager.AddFrequency(
				AppResources.OnceATwoYear,
				730,
				db);

			//RemoveDuplicates (db);
		}

		public string Version {
			get {
				return "2.2";
			}
		}

		#endregion


	}
}

