using System;
using Xamarin.Forms;
using SocialCapital.Data.Model;

namespace SocialCapital.Views.Converters
{
	public class ContactToStatusConverter : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (!(value is Contact))
				throw new ArgumentException ("Value must has type 'Contact'");
			
			if ((string)parameter == "UpdateStatus")
				return ContactToUpdateStatus (value as Contact);
			else
				throw new NotImplementedException (string.Format ("Parameter '{0}' is not implementer", parameter));
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ("Convert update status to contact is impossible");
		}

		string ContactToUpdateStatus(Contact contact)
		{
			return AppResources.PhoneContactUnknownStatus;

//			if (contact.CreateTime == null || contact.AddressBookUpdateTime == null)
//				return AppResources.PhoneContactUnknownStatus;
//			if (contact.CreateTime == contact.AddressBookUpdateTime)
//				return AppResources.PhoneContactImportedStatus;
//			else
//				return AppResources.PhoneContactUpdateStatus;
		}
	}
}

