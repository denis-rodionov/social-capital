using System;
using SQLite.Net.Attributes;
using Ninject;
using SocialCapital.Data.Model;
using SocialCapital.Data.Managers;
using SocialCapital.Data.Model.Enums;

namespace SocialCapital.Data.Model
{
	public class ContactTag : IHaveId, IEquatable<ContactTag>
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Indexed]
		public int TagId { get; set; }

		[Indexed]
		public int ContactId { get; set; }

		public override string ToString ()
		{
			return string.Format ("[ContactTag: Id={0}, TagId={1}, ContactId={2}]", Id, TagId, ContactId);
		}

		#region NavigationProperty

		private string tagName;
		[Ignore]
		public string TagName {
			get { 
				if (tagName == null)
					tagName = App.Container.Get<TagManager> ().GetTag (TagId).Name;
				return tagName;
			}
			set { tagName = value; }
		}

		#endregion

		#region IEquatable implementation
		public bool Equals (ContactTag other)
		{
			if (other == null)
				return false;

			return Id == other.Id;
		}

		public override int GetHashCode ()
		{
			return Id.GetHashCode ();
		}
		#endregion
	}
}

