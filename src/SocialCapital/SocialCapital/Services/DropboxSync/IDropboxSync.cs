using System;
using System.IO;

namespace SocialCapital.Services.DropboxSync
{
	public interface IDropboxSync
	{
		bool HasDropboxAccount();

		void SaveFile(byte[] data, string fileName);

		byte[] LoadFile(string fileName);
	}
}

