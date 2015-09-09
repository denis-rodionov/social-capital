using System;

namespace SocialCapital.Data.Model
{
	public enum EmailType {
		Home,
		Other,
		Work
	}

	/// <summary>
	/// Class migrated from phone Address book structure
	/// </summary>
	public class Email
	{
		public string Address { get; set; }

		public string Label { get; set; }

		public EmailType Type { get; set; }

		public Email ()
		{
		}

		public override string ToString ()
		{
			return string.Format ("[Address={0}, Label={1}, Type={2}]", Address, Label, Type);
		}
	}
}

