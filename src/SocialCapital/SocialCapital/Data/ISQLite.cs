using System;
using SQLite.Net;

namespace SocialCapital.Data
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection();
	}
}

