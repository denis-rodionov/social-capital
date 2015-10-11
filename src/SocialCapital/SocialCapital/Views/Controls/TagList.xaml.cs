using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SocialCapital.Data.Model;
using SocialCapital.ViewModels;
using SocialCapital.Views.Libs;
using System.Collections.Specialized;
using SocialCapital.Data.Model.Enums;
using System.Linq;
using System.Windows.Input;
using SocialCapital.Common;

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

	public class LabelContext {
		public ILabel Label { get; set; }
		public double Size { get; set; }
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


		/// <summary>
		/// Constructor
		/// </summary>
		public TagList ()
		{
			Size = 14;
			InitializeComponent ();
		}

		#region Properties

		public static BindableProperty LabelClickCommandProperty = BindableProperty.Create<TagList, ICommand>(
			x => x.LabelClickCommand, 
			null);

		/// <summary>
		/// Execute when clicked by any visible label.
		/// Send label name (string) as a command parameter.
		/// </summary>
		public ICommand LabelClickCommand {
			get { return (ICommand)this.GetValue(LabelClickCommandProperty); }
			set { this.SetValue(LabelClickCommandProperty, value); }
		}

		public static readonly BindableProperty TagsProperty =
			BindableProperty.Create<TagList, IEnumerable<ILabel>>(
				(tagList) => tagList.Tags,
				null,
				propertyChanged: (bindable, oldValue, newValue) => {
					(bindable as TagList).OnTagsModelChanged(oldValue, newValue);
				});

		public IEnumerable<ILabel> Tags { 
			set { 
				SetValue (TagsProperty, value); 
			}
			get { return (IEnumerable<ILabel>)GetValue (TagsProperty); }
		}

		public LayoutTypes LayoutType { 
			get { return layoutType; }
			set {
				InitLayout (value);					

				layoutType = value;

				if (BindingContext != null && BindingContext is IEnumerable<ILabel>)
					Fill (BindingContext as IEnumerable<ILabel>);
			}
		}

		public string Placeholder {
			get { return placeholder; }
			set {
				placeholder = value;
				placeholderLabel.Text = placeholder;
			}
		}

		public int Size {
			get;
			set;
		}

		#endregion

		#region Handlers

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (sender != null)
				UpdateComponent ((IEnumerable<ILabel>)sender);
		}

		public void OnTagsModelChanged(IEnumerable<ILabel> oldTags, IEnumerable<ILabel> newTags)
		{
			if (newTags is INotifyCollectionChanged)
				(newTags as INotifyCollectionChanged).CollectionChanged += OnCollectionChanged;
			
			if (newTags == null)
				return;

			if (oldTags == newTags)
				throw new ArgumentException("Stange behaviour of bindable property");

			UpdateComponent (newTags);
		}

		private void OnTapped(object sender, EventArgs args)
		{
			if (!(sender is View))
				throw new Exception ("sender is not a View (TagList)");
			
			var context = (sender as View).BindingContext as LabelContext;

			if (context != null && this.LabelClickCommand != null && this.LabelClickCommand.CanExecute(context.Label.Name)) {
				this.LabelClickCommand.Execute(context.Label.Name);
			}	
		}

		#endregion

		#region Implementation

		public void UpdateComponent(IEnumerable<ILabel> labels)
		{
			var timing = Timing.Start ("Show tag list");

			if (LayoutType == LayoutTypes.Undefined)
				throw new ArgumentException ("LayoutType is Undefined. Set value");

			//newTags.Tags.CollectionChanged += (s, ev) => {
			//	InitLayout (LayoutType);
			//	Fill (newTags);
			//};

			InitLayout (LayoutType);
			Fill (labels);

			timing.Finish (LogLevel.Trace);
		}

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

		void Fill (IEnumerable<ILabel> labels)
		{
			int count = 0;

			if (labels.Count() == 0 && Placeholder != null)
				ShowPlaceholder ();

			foreach (var label in labels) {
				var view = (View)ItemTemplate.CreateContent ();
				view.BindingContext = new LabelContext() { Label = label, Size = this.Size  };

				if (LayoutType == LayoutTypes.HorizontalGrid)
					AddElementToHorizontalGrid (view, count++);
				else if (LayoutType == LayoutTypes.Wrap)
					AddElementToWrap (view);
				else if (LayoutType == LayoutTypes.VerticalGrid)
					AddElementToVerticalGrid (view, count++);
				else
					throw new Exception ("Unknown LayoutType");
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

