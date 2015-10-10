using System;
using Xamarin.Forms;

namespace SocialCapital.Views.Converters
{
	public class StringToBoolConverter : IValueConverter
	{
		#region IValueConverter implementation
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var str = value as string;
			if (str == null)
				return false;
		
			return str.Length != 0;
		}
		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

