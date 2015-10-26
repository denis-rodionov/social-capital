using System;

namespace SocialCapital.Data.Migrations
{
	public class Migration_1_0 : IMigration
	{
		public Migration_1_0 ()
		{
		}

		#region IMigration implementation

		public void Migrate (IDataContext db)
		{
			db.Connection.Execute ("UPDATE Contact SET GroupId=NULL WHERE GroupId=0");
		}

		public string Version {
			get {
				return "1.0";
			}
		}

		#endregion
	}
}

