using System;
using SQLite.Net.Attributes;
using SocialCapital.Data.Model.Enums;

namespace SocialCapital.Data.Model
{
	public class Tag : IEquatable<Tag>, IHaveId, ILabel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Unique]
		public string Name { get; set; }

		#region IEquatable implementation

		public bool Equals (Tag obj)
		{
			if (obj == null) return false;

			return Name == obj.Name && Id == obj.Id;
		}

		public override int GetHashCode ()
		{
			return Name.GetHashCode ();
		}

		#endregion

		public override string ToString ()
		{
			return string.Format ("[Tag: Id={0}, Name={1}]", Id, Name);
		}
	}
}

