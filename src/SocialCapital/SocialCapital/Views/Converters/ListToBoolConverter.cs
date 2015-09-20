using System;
using Xamarin.Forms;
using System.Collections;

namespace SocialCapital.Views.Converters
{
	/// <summary>
	/// If list is empty returns false
	/// </summary>
	public class ListToBoolConverter : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return false;

			var not = false;
			if (parameter != null)
				not = bool.Parse ((string)parameter);
			
			var count = Count (value as IEnumerable);
			var res = (count != 0);

			return not ? !res : res;
		}
		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new Exception ("Cannot convert back");
		}

		int Count(IEnumerable source)
		{
			int res = 0;

			foreach (var item in source)
				res++;

			return res;
		}
	}
}

