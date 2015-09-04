using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SocialCapital.Data.Model;
using SocialCapital.ViewModels;
using SocialCapital.Views.Libs;

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

		/// <summary>
		/// Constructor
		/// </summary>
		public TagList ()
		{
			InitializeComponent ();

			BindingContextChanged += (object sender, EventArgs e) => BindingContextChangedHandler(sender, e);
		}

		public LayoutTypes LayoutType { 
			get { return layoutType; }
			set {
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

				layoutType = value;

				if (BindingContext != null && BindingContext is TagsVM)
					Fill (BindingContext as TagsVM);
			}
		}

		public void BindingContextChangedHandler(object sender, EventArgs e)
		{
			if (BindingContext == null)
				return;

			if (!(BindingContext is TagsVM))
				throw new ArgumentException ("Component TagList must have context of type TagsVM");

			if (LayoutType == LayoutTypes.Undefined)
				throw new ArgumentException ("LayoutType is Undefined. Set value");
			
			Fill (BindingContext as TagsVM);
		}

		#region Implementation

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

		void Fill (TagsVM tagList)
		{
			int count = 0;
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

