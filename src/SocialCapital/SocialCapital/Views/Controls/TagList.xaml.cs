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
		Grid,

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
				if (value == LayoutTypes.Grid) {
					CreateGrid ();
				} else if (value == LayoutTypes.Wrap) {
					CreateWrap ();
				} else
					throw new Exception ("Unsupported layout type");

				layoutType = value;

				if (BindingContext != null && BindingContext is IEnumerable<Tag>)
					Fill (BindingContext as IEnumerable<Tag>);
			}
		}

		public void BindingContextChangedHandler(object sender, EventArgs e)
		{
			if (BindingContext == null)
				return;

			if (!(BindingContext is IEnumerable<Tag>))
				throw new ArgumentException ("Component TagList must have context of type IEnumerable<Tag>");

			if (LayoutType == LayoutTypes.Undefined)
				throw new ArgumentException ("LayoutType is Undefined. Set value");
			
			Fill (BindingContext as IEnumerable<Tag>);
		}

		#region Implementation

		void CreateGrid()
		{
			gridContainer = new Grid ();
			gridContainer.RowDefinitions.Add (new RowDefinition () { Height = new GridLength(1, GridUnitType.Star) });
			Children.Add (gridContainer);
		}

		void CreateWrap()
		{
			wrapContainer = new WrapLayout () { Orientation = StackOrientation.Horizontal };
			Children.Add (wrapContainer);
		}

		void Fill (IEnumerable<Tag> tagList)
		{
			int count = 0;
			foreach (var tag in tagList) {
				var view = (View)ItemTemplate.CreateContent ();
				view.BindingContext = tag;

				if (LayoutType == LayoutTypes.Grid)
					AddElementToGrid (view, count++);
				else if (LayoutType == LayoutTypes.Wrap)
					AddElementToWrap (view);
				else
					throw new Exception ("Uncknown LayoutType");
			}
		}

		void AddElementToWrap (View view)
		{
			wrapContainer.Children.Add (view);
		}

		void AddElementToGrid (View view, int number)
		{
			gridContainer.ColumnDefinitions.Add (new ColumnDefinition () {
				Width = GridLength.Auto
			});
			gridContainer.Children.Add (view, number, 0);
		}

		#endregion
	}
}

