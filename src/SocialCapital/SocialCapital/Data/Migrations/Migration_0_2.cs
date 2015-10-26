using System;

namespace SocialCapital.Data.Migrations
{
	public class Migration_0_2 : IMigration
	{
		public Migration_0_2 ()
		{
		}

		#region IMigration implementation

		public void Migrate (IDataContext db)
		{
			db.Connection.Execute ("UPDATE Contact SET GroupId=NULL WHERE GroupId=0");
		}

		public string Version {
			get {
				return "0.2";
			}
		}

		#endregion
	}
}

