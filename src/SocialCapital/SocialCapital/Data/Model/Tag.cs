﻿using System;
using SQLite.Net.Attributes;

namespace SocialCapital.Data.Model
{
	public class Tag : IEquatable<Tag>
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

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

