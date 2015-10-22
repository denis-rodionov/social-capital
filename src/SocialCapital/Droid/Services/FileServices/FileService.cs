using System;
using SocialCapital.Services.FileService;
using Xamarin.Forms;

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

		#endregion
	}
}

