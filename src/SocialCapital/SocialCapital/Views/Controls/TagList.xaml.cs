using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SocialCapital.Data.Model;
using SocialCapital.ViewModels;

namespace SocialCapital.Views.Controls
{
	public partial class TagList : Repeater //: XLabs.Forms.Controls.RepeaterView<Tag>
	{
		public TagList ()
		{
			//var repeater = new XLabs.Forms.Controls.RepeaterView<string> ();

			InitializeComponent ();

			//BindingContext = new TagListVM ();
			var newCell = ItemTemplate.CreateContent();
			grid.Children.Add ((View)newCell, 0, 0);

			newCell = ItemTemplate.CreateContent ();
			grid.Children.Add ((View)newCell, 1, 0);

			newCell = ItemTemplate.CreateContent ();
			grid.Children.Add ((View)newCell, 2, 0);

			newCell = ItemTemplate.CreateContent ();
			grid.Children.Add ((View)newCell, 3, 0);

			//grid.Children.Add ((View)newCell);

			//ItemsView<string> p;
			//p.ItemTemplate
		}
	}
}

