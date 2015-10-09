using System;
using SocialCapital.Data.Model;
using System.Collections.Generic;

namespace SocialCapital.Data.Managers
{
	public class TagManager : BaseManager<Tag>
	{
		public TagManager ()
		{
		}

		public void SaveTags(IEnumerable<Tag> tags, DataContext db = null)
		{
			foreach (var tag in tags)
				if (tag.Id == 0)
					Insert (tag, db);
		}

		public Tag GetTag(int id)
		{
			return Get (id);
		}

		public List<Tag> GetTagList(Func<Tag, bool> whereClause)
		{
			return GetList (whereClause);
		}
	}
}

