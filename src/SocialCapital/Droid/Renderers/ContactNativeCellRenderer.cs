using System;
using Xamarin.Forms.Platform.Android;
//using Xamarin.Forms;
using SocialCapital.Views.Controls;
using SocialCapital.Droid.Renderers;
using Android.App;
using Android.Widget;
using Refractored.Controls;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Views;
using Xamarin.Forms;
using Android.Media;
using SocialCapital.Common;
using Android.Graphics.Drawables;
using System.Security.Cryptography;
using System.Linq;
using Android.Transitions;
using Android.Hardware.Camera2;
using Android.Content;
using Android.Media.Midi;
using System.ComponentModel;
using Android.Nfc.CardEmulators;
using Android.Util;

[assembly: ExportRenderer(typeof(ContactNativeCell), typeof(ContactNativeCellRenderer))]

namespace SocialCapital.Droid.Renderers
{
	public class ContactNativeCellRenderer : ViewCellRenderer
	{
		public ContactNativeCellRenderer ()
		{
		}

		protected override Android.Views.View GetCellCore (Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
		{
			//return base.GetCellCore (item, convertView, parent, context);

			var cell = (ContactNativeCell)item;

			var view = convertView;

			if (view == null)
			{// no view to re-use, create new
				view = (context as Activity).LayoutInflater.Inflate (Resource.Layout.ContactNativeCell, null);
			} else
			{ 
				// re-use, clear image
				// doesn't seem to help
				// view.FindViewById<ImageView> (Resource.Id.Image).Drawable.Dispose ();
			}

			UpdateView (view, cell, context);
			cell.View = view;

			return view;
		}

		protected override void OnCellPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnCellPropertyChanged (sender, e);
			var cell = (ContactNativeCell)Cell;
			var view = (Android.Views.View)cell.View;

			UpdateView (view, cell, null);
			view.Invalidate ();
		}

		private void UpdateView(Android.Views.View view, ContactNativeCell cell, Android.Content.Context context)
		{
			// Contact fields
			view.FindViewById<TextView> (Resource.Id.FullName).Text = cell.FullName;
			view.FindViewById<Android.Views.View> (Resource.Id.Stripe).SetBackgroundColor (cell.ColorStatus.ToAndroid ());

			// group
			view.FindViewById<TextView> (Resource.Id.GroupName).Visibility = cell.GroupName != null ? ViewStates.Visible : ViewStates.Invisible;
			view.FindViewById<TextView> (Resource.Id.GroupName).Text = cell.GroupName;

			// tags
			if (cell.Tags == null || cell.Tags.Count () == 0)
				view.FindViewById<TextView> (Resource.Id.Tags).Visibility = ViewStates.Invisible;
			else
			{
				view.FindViewById<TextView> (Resource.Id.Tags).Visibility = ViewStates.Visible;
				view.FindViewById<TextView> (Resource.Id.Tags).Text = string.Join (", ", cell.Tags.Select (t => t.Name));
			}

			// icon
			// TODO: grab images
			if (!String.IsNullOrWhiteSpace (cell.Icon)) {
				context.Resources.GetBitmapAsync (cell.Icon).ContinueWith ((t) => {
					var bitmap = t.Result;
					if (bitmap != null) {
						view.FindViewById<ImageView> (Resource.Id.Icon).SetImageBitmap (bitmap);
						bitmap.Dispose ();
					}
				}, TaskScheduler.FromCurrentSynchronizationContext() );

			} else {
				// clear the image
				view.FindViewById<ImageView> (Resource.Id.Icon).SetImageBitmap (null);
			}

			// Contact Image
			// TODO: grab images
			if (cell.ContactImage != null)
			{
				var bitmap = BitmapFactory.DecodeByteArray (cell.ContactImage, 0, cell.ContactImage.Length);
				view.FindViewById<CircleImageView> (Resource.Id.ContactImage).SetImageBitmap (bitmap);
			} else
				view.FindViewById<CircleImageView> (Resource.Id.ContactImage).SetImageResource (Resource.Drawable.avatar_placeholder);

			// checkbox
			view.FindViewById<Android.Views.View> (Resource.Id.Stripe).Visibility = ToVisibility (!cell.Selected.HasValue);
			view.FindViewById<ImageView> (Resource.Id.CheckboxImage).Visibility = ToVisibility (cell.Selected.HasValue);
			if (cell.Selected.HasValue)
			{
				if (cell.Selected.Value)
					view.FindViewById<ImageView> (Resource.Id.CheckboxImage).SetImageResource (Resource.Drawable.ic_check_circle_black_24dp);
				else
					view.FindViewById<ImageView> (Resource.Id.CheckboxImage).SetImageResource (Resource.Drawable.ic_radio_button_unchecked_black_24dp);

			}
		}

		private ViewStates ToVisibility(bool isVisible)
		{
			if (isVisible)
				return ViewStates.Visible;
			else
				return ViewStates.Invisible;
		}
	}
}

