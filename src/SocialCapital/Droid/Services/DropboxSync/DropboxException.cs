using System;

namespace SocialCapital.Droid.Services.DropboxSync
{
	public class DropboxException : Exception
	{
		public DropboxException (string message, Exception inner = null)
			: base (message, inner)
		{
		}
	}
}

