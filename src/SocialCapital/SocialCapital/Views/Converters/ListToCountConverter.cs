using System;
using Xamarin.Forms;
using System.Collections;

namespace SocialCapital.Views.Converters
{
	public class ListToCountConverter : IValueConverter
	{
		#region IValueConverter implementation
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return Count ((IEnumerable)value);
		}
		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
		#endregion

		int Count(IEnumerable source)
		{
			int res = 0;

			foreach (var item in source)
				res++;

			return res;
		}
	}
}

