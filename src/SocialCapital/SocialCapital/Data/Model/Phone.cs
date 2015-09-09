using System;

namespace SocialCapital.Data.Model
{
	public enum PhoneType {
		Home,
		HomeFax,
		Mobile,
		Other,
		Pager,
		Work,
		WorkFax
	}

	/// <summary>
	/// Class migrated from phone Address book structure
	/// </summary>
	public class Phone
	{
		public string Label { get; set; }

		public string Number { get; set; }

		public PhoneType Type { get; set; }

		public Phone ()
		{
		}

		public override string ToString ()
		{
			return string.Format ("[Label={0}, Number={1}, Type={2}]", Label, Number, Type);
		}
	}
}

