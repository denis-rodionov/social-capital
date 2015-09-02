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
			//var repeater = new XLabs.Forms.Controls.RepeaterView<string> ();

			InitializeComponent ();

			//BindingContext = new TagListVM ();
			var newView = (View)ItemTemplate.CreateContent();
			newView.BindingContext = new Tag () { Name = "Рыбалка" };
			grid.Children.Add (newView, 0, 0);

			newView = (View)ItemTemplate.CreateContent ();
			newView.BindingContext = new Tag () { Name = "Спорт" };
			grid.Children.Add (newView, 1, 0);

			newView = (View)ItemTemplate.CreateContent ();
			newView.BindingContext = new Tag () { Name = "Игры" };
			grid.Children.Add (newView, 2, 0);

			newView = (View)ItemTemplate.CreateContent ();
			newView.BindingContext = new Tag () { Name = "Футбол" };
			grid.Children.Add (newView, 3, 0);

			//grid.Children.Add ((View)newCell);

			//ItemsView<string> p;
			//p.ItemTemplate
		}
	}
}

