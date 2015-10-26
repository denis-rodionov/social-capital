using System;
using SocialCapital.Services.FileService;
using Xamarin.Forms;
using System.IO;

[assembly: Dependency(typeof(SocialCapital.Droid.Services.FileServices.FileService))]

namespace SocialCapital.Droid.Services.FileServices
{
	public class FileService : IFileService
	{
		public FileService ()
		{
		}

		#region IFileService implementation

		public void Delete (string path)
		{
			System.IO.File.Delete (path);
		}

		public void Copy(string pathSource, string pathDest)
		{
			File.Copy (pathSource, pathDest, true);
		}

		#endregion
	}
}

