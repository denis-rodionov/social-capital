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
			CandidateTags = new List<Tag> ();

			Add = new Command (
				execute: (obj) => AddTag(obj),
				canExecute: (obj) => {
					return !string.IsNullOrWhiteSpace(SearchTag);
				});

			Delete = new Command (
				execute: (obj) => DeleteTag(obj));
		}

		#endregion

		#region Properties

		private ObservableCollection<Tag> tags;
		public ObservableCollection<Tag> Tags { 
			get { return tags; }
			private set { 
				SetProperty (ref tags, value); 
				OnPropertyChanged ("TagList");
			}
		}

		private List<Tag> candidateTags;
		public List<Tag> CandidateTags { 
			get { return candidateTags; }
			set { SetProperty(ref candidateTags, value); }
		}

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

		#region Commands

		private void DeleteTag(object obj)
		{
			var tag = obj as Tag;
			Tags.Remove (tag);
		}

		private void AddTag(object obj)
		{
			var candidate = CandidateTags.SingleOrDefault(t => t.Name == (string)obj);

			if (candidate != null)
				Tags.Add(candidate);
			else
				Tags.Add(new Tag() { Name = (string)obj });

			SearchTag = null;
		}

		#endregion

		#region Implementation

		public void FillCandidateTags(string filter)
		{
			var res = new List<Tag> ();

			if (!string.IsNullOrEmpty(filter))
			{
				res = App.Container.Get<TagManager> ().GetTagList (t => t.Name.ToLowerInvariant ().Contains (filter.ToLowerInvariant ()));
			}

			if (CandidateTags.Count() != res.Count())
				CandidateTags = res;
		}

		#endregion
	}
}

