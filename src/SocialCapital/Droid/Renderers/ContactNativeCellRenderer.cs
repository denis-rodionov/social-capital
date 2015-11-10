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
				//view.FindViewById<ImageView> (Resource.Id.Image).Drawable.Dispose ();
			}

			view.FindViewById<TextView> (Resource.Id.FullName).Text = cell.FullName;
			view.FindViewById<Android.Views.View> (Resource.Id.Stripe).SetBackgroundColor (cell.ColorStatus.ToAndroid ());

			if (cell.ContactImage != null)
			{
				var bitmap = BitmapFactory.DecodeByteArray (cell.ContactImage, 0, cell.ContactImage.Length);
				view.FindViewById<CircleImageView> (Resource.Id.ContactImage).SetImageBitmap (bitmap);
			} else
				view.FindViewById<CircleImageView> (Resource.Id.ContactImage).SetImageResource (Resource.Drawable.avatar_placeholder);

			return view;
		}
	}
}

