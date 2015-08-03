using System;
using System.Reflection;
using System.IO;

namespace SocialCapital
{
	public class ResourceLoader
	{
		public ResourceLoader ()
		{
		}

		public static byte[] LoadFileFromResource(string resourceName)
		{
			var assembly = typeof(ResourceLoader).GetTypeInfo ().Assembly;
			var stream = assembly.GetManifestResourceStream (resourceName);

			using (var ms = new MemoryStream())
			{
				stream.CopyTo (ms);
				return ms.ToArray ();
			}
		}

	}
}

