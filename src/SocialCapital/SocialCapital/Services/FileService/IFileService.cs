using System;

namespace SocialCapital.Services.FileService
{
	public interface IFileService
	{
		void Delete(string path);

		void Copy(string pathSource, string pathDest);
	}
}

