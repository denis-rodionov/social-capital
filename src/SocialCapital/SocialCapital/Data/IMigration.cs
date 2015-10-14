using System;

namespace SocialCapital.Data
{
	public interface IMigration
	{
		string Version { get; }

		void Migrate(IDataContext db);
	}
}

