using System;
using SocialCapital.Data.Managers;
using Ninject;
using SocialCapital.Data.Model;

namespace SocialCapital.Data.Migrations
{
	public class Migration_0_1 : IMigration
	{
		private FrequencyManager frequencyManager;
		
		public Migration_0_1 ()
		{
			frequencyManager = App.Container.Get<FrequencyManager> ();
		}

		#region IMigration implementation

		public void Migrate (IDataContext db)
		{
			// twice a month
			frequencyManager.AddFrequency(
					AppResources.TwiceAMonth,
					15,
					db);
		}

		public string Version {
			get {
				return "0.2";
			}
		}

		#endregion
	}
}

