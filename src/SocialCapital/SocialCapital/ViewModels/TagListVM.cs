using System;
using System.Collections.Generic;
using SocialCapital.Data.Model;
using System.Collections.ObjectModel;

namespace SocialCapital.ViewModels
{
	public class TagListVM 
	{
		public ObservableCollection<Tag> Tags { get; set; }

		public TagListVM()
		{
			//Tags = new List<Tag> () {new Tag() { Name = "Осень" }, new Tag() { Name = "Зима" }};
		}
	}
}


