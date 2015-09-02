using System;
using Xamarin.Forms;

namespace SocialCapital.Views.Controls
{
	public class Repeater : StackLayout
	{
		public Repeater ()
		{
		}

		public DataTemplate ItemTemplate { get; set; }
	}
}

