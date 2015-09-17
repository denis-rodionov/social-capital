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

			StartIntent (intent);
		}

		public bool WriteSmS(string number, string smsBody = "")
		{
			var smsUri = Android.Net.Uri.Parse("smsto:" + number);

			var smsIntent = new Intent (Intent.ActionSendto, smsUri);

			smsIntent.PutExtra ("sms_body", smsBody);  
			StartIntent (smsIntent);
		}

		#endregion

		static void StartIntent (Intent intent)
		{
			var context = Forms.Context;
			if (context == null) {
				Log.GetLogger ().Log ("PhoneService.Call : No context!", LogLevel.Error);
				return false;
			}

			if (IsIntentAvailable (context, intent)) {
				context.StartActivity (intent);
				return true;
			}
			else {
				Log.GetLogger ().Log ("PhoneService.Call : Intent is not available!", LogLevel.Error);
				return false;
			}
		}

		public static bool IsIntentAvailable(Context context, Intent intent) {

			var packageManager = context.PackageManager;

			var list = packageManager.QueryIntentServices(intent, 0)
				.Union(packageManager.QueryIntentActivities(intent, 0));
			if (list.Any())
				return true;

			TelephonyManager mgr = TelephonyManager.FromContext(context);
			return mgr.PhoneType != PhoneType.None;
		}
	}
}

