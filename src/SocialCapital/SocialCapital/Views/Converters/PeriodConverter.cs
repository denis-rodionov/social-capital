using System;
using Xamarin.Forms;

namespace SocialCapital.Views.Converters
{
	public class PeriodConverter : IValueConverter
	{
		#region IValueConverter implementation
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var period = (PeriodValues)value;
			switch (period)
			{
				case PeriodValues.Day:
					return AppResources.Day;
				case PeriodValues.Week:
					return AppResources.Week;
				case PeriodValues.Month:
					return AppResources.Month;
				case PeriodValues.ThreeMonth:
					return AppResources.ThreeMonth;
				case PeriodValues.Year:
					return AppResources.Year;
				case PeriodValues.TwoYear:
					return AppResources.TwoYears;
				case PeriodValues.Never:
					return AppResources.Never;
				default:
					throw new Exception ("Unknown period value: " + period);
			}
		}
		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var period = (string)value;
			return PeriodValues.Day;	// TODO: implement back converting
		}
		#endregion
		
	}
}

