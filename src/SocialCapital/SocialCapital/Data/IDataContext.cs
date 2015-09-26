using System;
using SQLite.Net;

namespace SocialCapital.Data
{
	public interface IDataContext : IDisposable
	{
		SQLiteConnection Connection { get; }
	}
}

