using System;

using Xamarin.Forms;

namespace SocialCapital.Views.Converters
{
	public class DateTimeCustomConverter : IValueConverter
	{
		#region IValueConverter implementation
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var time = (DateTime)value;
			return time.ToAgoFormatRus ();
		}
		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}


