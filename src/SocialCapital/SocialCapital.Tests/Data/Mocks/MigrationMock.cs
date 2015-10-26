using System;
using SocialCapital.Data;

namespace SocialCapital.Tests.Data.Mocks
{
	public class MigrationMock : IMigration
	{
		public MigrationMock (string version)
		{
			Done = false;
			Version = version;
		}

		public bool Done { get; set; }

		public string Version { get; set; }

		#region IMigration implementation

		public void Migrate (IDataContext db)
		{
			Done = true;
		}

		#endregion
	}
}

