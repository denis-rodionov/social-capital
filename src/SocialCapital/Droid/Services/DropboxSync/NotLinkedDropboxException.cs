using System;

namespace SocialCapital.Droid.Services.DropboxSync
{
	public class NotLinkedDropboxException : DropboxException
	{
		public NotLinkedDropboxException ()
			:base("No dropbox account linked to the device")
		{
		}
	}
}

