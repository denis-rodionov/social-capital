using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace SocialCapital.Views.Converters
{
	public class PeriodConverter : IValueConverter
	{
		Dictionary<PeriodValues, string> Dict { get; set; }

		public PeriodConverter()
		{
			Dict = new Dictionary<PeriodValues, string> ();

			Dict.Add (PeriodValues.Day, AppResources.Day);
			Dict.Add (PeriodValues.Week, AppResources.Week);
			Dict.Add (PeriodValues.Month, AppResources.Month);
			Dict.Add (PeriodValues.ThreeMonth, AppResources.ThreeMonth);
			Dict.Add (PeriodValues.Year, AppResources.Year);
			Dict.Add (PeriodValues.TwoYear, AppResources.TwoYears);
			Dict.Add (PeriodValues.Never, AppResources.Never);
			
		}

		#region IValueConverter implementation
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var period = (PeriodValues)value;
			if (Dict.ContainsKey (period))
				return Dict [period];
			else
				throw new Exception ("Unknown period value: " + period);
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var period = (string)value;

			if (Dict.ContainsValue (period))
				return Dict.Keys.Single<PeriodValues> (k => Dict [k] == period);
			else
				throw new Exception ("Cannot convert back: Unknown period value: " + period);
		}
		#endregion
		
	}
}

