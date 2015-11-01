using System;
using Android.Views;
using Xamarin.Forms;
using SocialCapital.Views.Controls;
using Xamarin.Forms.Platform.Android;

namespace SocialCapital.Droid.Renderers.FastCellNs
{
	internal sealed class NativeCell : ViewGroup
	{

		public NativeCell (Android.Content.Context context, FastCell fastCell) : base (context)
		{
			FastCell = fastCell;
			fastCell.PrepareCell ();
			var renderer = RendererFactory.GetRenderer (fastCell.View);
			this.AddView (renderer.ViewGroup);
			//			_view = renderer.NativeView;
			//			ContentView.AddSubview (_view);
		}

		public FastCell FastCell {
			get;
			set;
		}

		Size _lastSize;

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			if (changed) {
				//TODO
			}
		}

		protected override void OnSizeChanged (int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged (w, h, oldw, oldh);
			//TODO update sizes of the xamarin view
			var newSize = new Size (w, h);
			if (_lastSize.Equals (Size.Zero) || !_lastSize.Equals (newSize)) {

				//				var layout = FastCell.Content;
				var layout = FastCell.View as Layout<Xamarin.Forms.View>;
				if (layout != null) {
					layout.Layout (new Rectangle (0, 0, w, h));
					layout.ForceLayout ();
					FixChildLayouts (layout);
				}
				_lastSize = newSize;
			}

			//TODO set the frame size
		}

		void FixChildLayouts (Layout<Xamarin.Forms.View> layout)
		{
			foreach (var child in layout.Children) {
				if (child is StackLayout) {
					((StackLayout)child).ForceLayout ();
					FixChildLayouts (child as Layout<Xamarin.Forms.View>);
				}
				if (child is Xamarin.Forms.AbsoluteLayout) {
					((Xamarin.Forms.AbsoluteLayout)child).ForceLayout ();
					FixChildLayouts (child as Layout<Xamarin.Forms.View>);
				}
			}
		}
	}

}

