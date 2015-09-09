using System;

namespace SocialCapital.Data.Model
{
	/// <summary>
	/// Class migrated from phone Address book structure
	/// </summary>
	public class Organization
	{
		public string ContactTitle { get; set; }

		public string Name { get; set; }

		public string Label { get; set; }

		public Organization ()
		{
		}

		public override string ToString ()
		{
			return string.Format ("[ContactTitle={0}, Name={1}, Label={2}]", ContactTitle, Name, Label);
		}
	}
}

