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

			if (view == null) {// no view to re-use, create new
				view = (context as Activity).LayoutInflater.Inflate (Resource.Layout.ContactNativeCell	, null);
			} else { 
				// re-use, clear image
				// doesn't seem to help
				//view.FindViewById<ImageView> (Resource.Id.Image).Drawable.Dispose ();
			}

			view.FindViewById<TextView> (Resource.Id.FullName).Text = cell.FullName;
			view.FindViewById<Android.Views.View> (Resource.Id.Stripe).SetBackgroundColor (cell.ColorStatus.ToAndroid());

			if (cell.ContactImage != null)
			{
				var bitmap = GetImageFromImageSource (cell.ContactImage).Result;
				view.FindViewById<CircleImageView> (Resource.Id.ContactImage).SetImageBitmap (bitmap);
			}

			return view;
		}

		private async Task<Bitmap> GetImageFromImageSource(ImageSource imageSource)
		{
			var context = Forms.Context;
			IImageSourceHandler handler;

			if (imageSource is FileImageSource)
			{
				handler = new FileImageSourceHandler();
			}
			else if (imageSource is StreamImageSource)
			{
				handler = new StreamImagesourceHandler(); // sic
			}
			else if (imageSource is UriImageSource)
			{
				handler = new ImageLoaderSourceHandler(); // sic
			}
			else
			{
				throw new NotImplementedException();
			}

			return await handler.LoadImageAsync(imageSource, context);
		}
	}
}

