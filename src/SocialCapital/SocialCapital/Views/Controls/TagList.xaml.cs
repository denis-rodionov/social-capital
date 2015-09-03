using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SocialCapital.Data.Model;
using SocialCapital.ViewModels;
using SocialCapital.Views.Libs;

namespace SocialCapital.Views.Controls
{
	public partial class TagList : Repeater //: XLabs.Forms.Controls.RepeaterView<Tag>
	{


		public TagList ()
		{
			InitializeComponent ();

			BindingContextChanged += (object sender, EventArgs e) => BindingContextChangedHandler(sender, e);
			Log.GetLogger ().Log ("BindingContext={0}", BindingContext);


		}

		public void BindingContextChangedHandler(object sender, EventArgs e)
		{
			if (BindingContext == null)
				return;

			if (!(BindingContext is IEnumerable<Tag>))
				throw new ArgumentException ("Component TagList must have context of type IEnumerable<Tag>");
			
			var tagList = (IEnumerable<Tag>)BindingContext;

			int count = 0;
			foreach (var tag in tagList)
			{
				var view = (View)ItemTemplate.CreateContent();
				view.BindingContext = tag;
				grid.ColumnDefinitions.Add(new ColumnDefinition() { Width=GridLength.Auto });
				grid.Children.Add (view, count++, 0);
			}

			/*
			var newView = (View)ItemTemplate.CreateContent ();
			newView.BindingContext = new Tag () { Name = "Спорт" };
			grid.Children.Add (newView, 1, 0);

			newView = (View)ItemTemplate.CreateContent ();
			newView.BindingContext = new Tag () { Name = "Игры" };
			grid.Children.Add (newView, 2, 0);

			newView = (View)ItemTemplate.CreateContent ();
			newView.BindingContext = new Tag () { Name = "Футбол" };
			grid.Children.Add (newView, 3, 0);
			*/
		}
	}
}

