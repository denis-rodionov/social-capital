using System;

namespace SocialCapital
{
	public class TagManager
	{
		public TagManager ()
		{
		}

		public void Init()
		{
			using (var db = new DataContext ()) {
				if (db.Connection.Table<Tag> ().Count () == 0) {
					db.Connection.Insert (new Tag () { Name = "Футбол" });
					db.Connection.Insert (new Tag () { Name = "Лыжи" });
				}
			}
		}
	}
}

