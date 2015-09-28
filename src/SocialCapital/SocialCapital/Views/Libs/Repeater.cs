using System;
using Xamarin.Forms;
using SocialCapital.Logging;
using System.Collections.Generic;

namespace SocialCapital.Views.Libs
{
	public class Repeater : ContentView
	{
		public Repeater ()
		{
			//BindingContextChanged += (object sender, EventArgs e) => {
			//	Log.GetLogger().Log("BindingContext set: sender={0}, sender.BindingContext={1}", sender, (sender as BindableObject).BindingContext);
			//};
		}

		public DataTemplate ItemTemplate { get; set; }
	}

//	public class TableRepeater : IEnumerable<Cell>
//	{
//		public DataTemplate ItemTemplate { get; set; }
//	}
}

