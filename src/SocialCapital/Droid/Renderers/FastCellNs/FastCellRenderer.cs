using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Java.Util;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;
using SocialCapital.Views.Controls;
using SocialCapital.Droid.Renderers.FastCellNs;

[assembly: ExportRenderer (typeof(FastCell), typeof(FastCellRenderer))]
namespace SocialCapital.Droid.Renderers.FastCellNs
{
	public class FastCellRenderer : ViewCellRenderer
	{
		//TODO add a lookup for the cells we piggy back of.
		protected override Android.Views.View GetCellCore (Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
		{
			var cellCache = FastCellCache.Instance.GetCellCache (parent);
			var fastCell = item as FastCell;
			Android.Views.View cellCore = convertView;
			if (cellCore != null && cellCache.IsCached (cellCore)) {
				cellCache.RecycleCell (cellCore, fastCell);
			} else {
				if (!fastCell.IsInitialized) {
					fastCell.PrepareCell ();
				}
				cellCore = base.GetCellCore (fastCell, convertView, parent, context);
				cellCache.CacheCell (fastCell, cellCore);
			}
			return cellCore;
		}


	}
}

