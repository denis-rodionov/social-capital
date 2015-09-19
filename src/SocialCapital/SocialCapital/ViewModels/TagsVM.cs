using System;
using System.Linq;
using System.Collections.ObjectModel;
using SocialCapital.Data.Model;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;

namespace SocialCapital.ViewModels
{
	public class TagsVM : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<Tag> Tags { get; private set; }

		public ICommand Add { get; private set; }

		public ICommand Delete { get; private set; }

		private string searchTag = null;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="tags">Tags.</param>
		public TagsVM (IEnumerable<Tag> tags)
		{
			Tags = new ObservableCollection<Tag> (tags);

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

		public string TagList { 
			get { 
				//Tags.PropertyChanged += (sender, e) => { OnPropertyChanged(); };
				return string.Join (",", Tags.Select(t => t.Name).ToArray ()); 
			} 
		}

		public string SearchTag {
			get { return searchTag; }
			set {
				if (searchTag != value) {
					searchTag = value;
					FirePropertyChanged ("SearchTag");
					(Add as Command).ChangeCanExecute ();
				}
			}
		}

		public void FirePropertyChanged(string property)
		{
			var handler = PropertyChanged;

			if (handler != null)
				handler (this, new PropertyChangedEventArgs (property));
		}
	}
}

