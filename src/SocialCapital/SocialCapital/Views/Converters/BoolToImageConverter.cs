using System;
using Xamarin.Forms;

namespace SocialCapital.Views.Converters
{
	public class BoolToImageConverter :IValueConverter
	{
		#region IValueConverter implementation
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if ((bool)value)
				return "ic_check_circle_black_24dp.png";
			else
				return "ic_radio_button_unchecked_black_24dp.png";
		}
		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

