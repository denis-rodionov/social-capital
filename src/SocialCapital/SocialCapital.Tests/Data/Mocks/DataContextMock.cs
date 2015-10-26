using System;
using SocialCapital.Data;
using SQLite.Net;

namespace SocialCapital.Tests.Data.Mocks
{
	public class DataContextMock : IDataContext
	{
		public DataContextMock ()
		{
		}

		#region IDataContext implementation

		public void CloseConnection ()
		{
			throw new NotImplementedException ();
		}

		public SQLiteConnection Connection {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
		}

		#endregion
	}
}

