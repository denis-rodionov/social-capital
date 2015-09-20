using System;
using Xamarin.Forms;
using SocialCapital.Data.Model.Enums;

namespace SocialCapital.Views.Converters
{
	public class CommunicationToImageConverter : IValueConverter
	{
		#region IValueConverter implementation
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var type = (CommunicationType)value;

			switch (type)
			{
				case CommunicationType.PhoneCall:
					return "ic_phone_black_18dp.png";
				case CommunicationType.SmsSend:
					return "ic_textsms_black_18dp.png";
				case CommunicationType.EmailSent:
					return "ic_email_black_18db.png";
				default:
					throw new Exception ("No image for type " + type);
			}
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

