using System;
using Xamarin.Forms;

namespace SocialCapital.Views.Libs
{
	public class Repeater : ContentView
	{
		public Repeater ()
		{
			BindingContextChanged += (object sender, EventArgs e) => {
				Log.GetLogger().Log("BindingContext set: sender={0}, sender.BindingContext={1}", sender, (sender as BindableObject).BindingContext);
			};
		}

		public DataTemplate ItemTemplate { get; set; }
	}
}

