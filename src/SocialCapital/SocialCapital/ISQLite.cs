using System;
using SQLite.Net;

namespace SocialCapital
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection();
	}
}

