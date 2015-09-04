using System;
using System.Linq;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using System.Collections.Generic;

namespace SocialCapital.ViewModels
{
	public class TagsVM
	{
		public ObservableCollection<Tag> Tags { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="tags">Tags.</param>
		public TagsVM (IEnumerable<Tag> tags)
		{
			Tags = new ObservableCollection<Tag> (tags);
		}
	}
}

