using System;
using System.Linq;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using SocialCapital.Data.Managers;
using Ninject;

namespace SocialCapital.ViewModels
{
	public class TagsVM : ViewModelBase
	{
		public ICommand Add { get; private set; }

		public ICommand Delete { get; private set; }

		#region Init

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="tags">Tags.</param>
		public TagsVM (IEnumerable<Tag> tags)
		{
			Tags = new ObservableCollection<Tag> (tags);
			CandidateTags = new ObservableCollection<Tag> ();

			Add = new Command (
				execute: (obj) => {
					var newTag = (obj as Entry).Text;
					Tags.Add(new Tag() { Name = newTag });
					SearchTag = null;
				},
				canExecute: (obj) => {
					return !string.IsNullOrWhiteSpace(SearchTag);
				});

			Delete = new Command (
				execute: (obj) => {
					var tag = obj as Tag;
					Tags.Remove (tag);
				});
		}

		#endregion

		#region Properties

		public ObservableCollection<Tag> Tags { get; private set; }

		public ObservableCollection<Tag> CandidateTags { get; private set; }

		public string TagList { 
			get {
				return string.Join (",", Tags.Select(t => t.Name).ToArray ()); 
			} 
		}

		private string searchTag = null;
		public string SearchTag {
			get { return searchTag; }
			set {	
				SetProperty (ref searchTag, value);
				(Add as Command).ChangeCanExecute ();
				FillCandidateTags (searchTag);
			}
		}

		#endregion

		#region Implementation

		public void FillCandidateTags(string filter)
		{
			CandidateTags.Clear ();

			var tags = App.Container.Get<TagManager> ().GetTagList (t => t.Name.ToLowerInvariant ().Contains (filter.ToLowerInvariant ()));
			//var toAdd = tags.Except (CandidateTags);
			//var toDelete = CandidateTags.Except (tags);

			foreach (var tag in tags)
				CandidateTags.Add (tag);

		}

		#endregion
	}
}

