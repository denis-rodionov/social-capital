using System;
using Xamarin.Forms;
using SocialCapital.PhoneServices;
using Android.Telephony;
using Android.Content;
using System.Linq;

[assembly: Dependency(typeof(SocialCapital.Droid.Services.PhoneService))]

namespace SocialCapital.Droid.Services
{	
	public class PhoneService : IPhoneService
	{
		#region IPhoneService

		public bool Call(string number)
		{
			var intent = new Intent(Intent.ActionCall);
			intent.SetData(Android.Net.Uri.Parse("tel:" + number));

			return StartIntent (intent);
		}

		public bool WriteSmS(string number, string smsBody = "")
		{
			var smsUri = Android.Net.Uri.Parse("smsto:" + number);

			var smsIntent = new Intent (Intent.ActionSendto, smsUri);

			smsIntent.PutExtra ("sms_body", smsBody);  
			return StartIntent (smsIntent);
		}

		public bool SendEmail (string toAddress, string subject = "", string text = "")
		{
			var email = new Intent (Android.Content.Intent.ActionSend);

			email.PutExtra (Android.Content.Intent.ExtraEmail, toAddress);
			email.PutExtra (Android.Content.Intent.ExtraSubject, subject);
			email.PutExtra (Android.Content.Intent.ExtraText, text);

			email.SetType ("message/rfc822");

			return StartIntent (email);
		}

		#endregion

		static bool StartIntent (Intent intent)
		{
			var context = Forms.Context;
			if (context == null) {
				Log.GetLogger ().Log ("PhoneService : No context!", LogLevel.Error);
				return false;
			}

			if (IsIntentAvailable (context, intent)) {
				context.StartActivity (intent);
				return true;
			}
			else {
				Log.GetLogger ().Log ("PhoneService : Intent is not available!", LogLevel.Error);
				return false;
			}
		}

		public static bool IsIntentAvailable(Context context, Intent intent) {

			var packageManager = context.PackageManager;

			var services = packageManager.QueryIntentServices (intent, 0);
			var activities = packageManager.QueryIntentActivities (intent, 0);

			var list = services.Union (activities);
			if (list.Any())
				return true;

			TelephonyManager mgr = TelephonyManager.FromContext(context);
			return mgr.PhoneType != PhoneType.None;
		}
	}
}

