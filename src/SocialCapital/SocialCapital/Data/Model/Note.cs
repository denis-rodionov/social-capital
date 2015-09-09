using System;

namespace SocialCapital.Data.Model
{
	/// <summary>
	/// Class migrated from phone Address book structure
	/// </summary>
	public class Note
	{
		public string Contents { get; set; }

		public Note ()
		{
		}

		public override string ToString ()
		{
			return string.Format ("[{0}]", Contents);
		}
	}
}

