using System;
using System.IO;
using System.Collections.Generic;

namespace SocialCapital.Services.DropboxSync
{
	public interface IDropboxSync
	{
		/// <summary>
		/// Determines whether this instance has dropbox account.
		/// </summary>
		/// <returns><c>true</c> if this instance has dropbox account; otherwise, <c>false</c>.</returns>
		bool HasDropboxAccount();

		/// <summary>
		/// Uploads binary data to dropbox
		/// </summary>
		/// <param name="data">Binary data</param>
		/// <param name="dropboxFileName">desired file name in dropbox</param>
		void UploadFile(byte[] data, string dropboxFileName);

		/// <summary>
		/// Uploads the file from the given path to dropbox
		/// </summary>
		/// <param name="path">Device path to the file</param>
		/// <param name="dropboxFileName">Dropbox file name</param>
		void UploadFile(string path, string dropboxFileName);

		/// <summary>
		/// Downloads the file with the given name from dropbox
		/// </summary>
		/// <returns>Binary data of the file</returns>
		/// <param name="fileName">The file name in dropbox</param>
		byte[] DownloadData(string fileName);

		/// <summary>
		/// Downloads the file.
		/// </summary>
		/// <returns>The device file path</returns>
		/// <param DownloadFile="fileName">File name in dropbox</param>
		void DownloadFile(string fileName, string destinationPath);

		/// <summary>
		/// Gets all files in the app folder
		/// </summary>
		/// <returns>The file infos.</returns>
		IEnumerable<DropboxFile> GetFileInfos();
	}
}

