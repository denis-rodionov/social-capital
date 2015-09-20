using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SocialCapital.Data.Model;
using SocialCapital.ViewModels;
using SocialCapital.Views.Libs;
using System.Collections.Specialized;

namespace SocialCapital.Views.Controls
{
	/// <summary>
	/// 
	/// </summary>
	public enum LayoutTypes {
		/// <summary>
		/// Need to set the property
		/// </summary>
		Undefined,

		/// <summary>
		/// One-row grid of elements.
		/// Fixed Height.
		/// </summary>
		HorizontalGrid,

		/// <summary>
		/// One-column grid of elements
		/// </summary>
		VerticalGrid,

		/// <summary>
		/// Wraps to show all the elements.
		/// Variable height.
		/// </summary>
		Wrap
	}



	/// <summary>
	/// Component for visualizing contact tag list
	/// </summary>
	public partial class TagList : Repeater //: XLabs.Forms.Controls.RepeaterView<Tag>
	{
		private LayoutTypes layoutType = LayoutTypes.Undefined;
		private Grid gridContainer = null;
		private WrapLayout wrapContainer = null;
		private string placeholder = null;



		public static readonly BindableProperty TagsProperty =
			BindableProperty.Create<TagList, TagsVM>(
				(tagList) => tagList.Tags,
				null,
				propertyChanged: (bindable, oldValue, newValue) => {
					(bindable as TagList).OnTagsModelChanged(oldValue, newValue);
				});

		/// <summary>
		/// Constructor
		/// </summary>
		public TagList ()
		{
			InitializeComponent ();
		}

		public TagsVM Tags { 
			set { SetValue (TagsProperty, value); }
			get { return (TagsVM)GetValue (TagsProperty); }
		}

		public LayoutTypes LayoutType { 
			get { return layoutType; }
			set {
				InitLayout (value);					

				layoutType = value;

				if (BindingContext != null && BindingContext is TagsVM)
					Fill (BindingContext as TagsVM);
			}
		}

		public string Placeholder {
			get { return placeholder; }
			set {
				placeholder = value;
				placeholderLabel.Text = placeholder;
			}
		}

		public void OnTagsModelChanged(TagsVM oldTags, TagsVM newTags)
		{
			if (newTags == null)
				return;

			if (oldTags == newTags)
				throw new ArgumentException("Stange behaviour of bindable property");

			if (LayoutType == LayoutTypes.Undefined)
				throw new ArgumentException ("LayoutType is Undefined. Set value");
			
			//newTags.Tags.CollectionChanged += (s, ev) => {
			//	InitLayout (LayoutType);
			//	Fill (newTags);
			//};

			InitLayout (LayoutType);
			Fill (newTags);
		}

		#region Implementation

		void InitLayout (LayoutTypes value)
		{
			switch (value) {
			case LayoutTypes.HorizontalGrid:
				CreateHorizontalGrid ();
				break;
			case LayoutTypes.VerticalGrid:
				CreateVerticalGrid ();
				break;
			case LayoutTypes.Wrap:
				CreateWrap ();
				break;
			default:
				throw new Exception ("Unsupported layout type");
			}
		}

		void CreateHorizontalGrid()
		{
			gridContainer = new Grid ();
			gridContainer.RowDefinitions.Add (new RowDefinition () { Height = new GridLength(1, GridUnitType.Star) });
			Content = gridContainer;
		}

		void CreateVerticalGrid()
		{
			gridContainer = new Grid ();
			gridContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
			Content = gridContainer;
		}

		void CreateWrap()
		{
			wrapContainer = new WrapLayout () { Orientation = StackOrientation.Horizontal };
			Content = wrapContainer;
		}

		void ShowPlaceholder ()
		{
			Content = placeHolderContaineer;
		}

		void Fill (TagsVM tagList)
		{
			int count = 0;

			if (tagList.Tags.Count == 0 && Placeholder != null)
				ShowPlaceholder ();

			foreach (var tag in tagList.Tags) {
				var view = (View)ItemTemplate.CreateContent ();
				view.BindingContext = tag;

				if (LayoutType == LayoutTypes.HorizontalGrid)
					AddElementToHorizontalGrid (view, count++);
				else if (LayoutType == LayoutTypes.Wrap)
					AddElementToWrap (view);
				else if (LayoutType == LayoutTypes.VerticalGrid)
					AddElementToVerticalGrid (view, count++);
				else
					throw new Exception ("Uncknown LayoutType");
			}
		}

		void AddElementToWrap (View view)
		{
			wrapContainer.Children.Add (view);
		}

		void AddElementToHorizontalGrid (View view, int number)
		{
			gridContainer.ColumnDefinitions.Add (new ColumnDefinition () {
				Width = GridLength.Auto
			});
			gridContainer.Children.Add (view, number, 0);
		}

		void AddElementToVerticalGrid (View view, int number)
		{
			gridContainer.RowDefinitions.Add (new RowDefinition () {
				Height = GridLength.Auto
			});
			gridContainer.Children.Add (view, 0, number);
		}

		#endregion
	}
}

