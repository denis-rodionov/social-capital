using System;
using Xamarin.Forms;
using SocialCapital.Data.Model;
using SocialCapital.Data.Managers;
using Ninject;

namespace SocialCapital.Views.Converters
{
	public class FrequencyConverter : IValueConverter
	{
		#region IValueConverter implementation
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var frequency = (Frequency)value;
			return frequency.Name;
		}
		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return App.Container.Get<FrequencyManager> ().GetFrequency ((string)value);
		}
		#endregion
	}
}

