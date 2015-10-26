using System;
using Xamarin.Forms;
using SocialCapital.Common.Interfaces;

namespace SocialCapital.Views.Converters
{
	public class CommaSeparatedConverter : IValueConverter
	{
		#region IValueConverter implementation
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var strings = (IStringList)value;
			var res = string.Join (",", strings.GetStrings()); 
			return res;
		}
		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

