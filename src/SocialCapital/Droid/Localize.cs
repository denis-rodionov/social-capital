using System;
using Xamarin.Forms;
using System.Threading;
using System.Globalization;

[assembly:Dependency(typeof(SocialCapital.Droid.Localize))]

namespace SocialCapital.Droid
{
	public class Localize : SocialCapital.ILocalize
	{
		public CultureInfo GetCurrentCultureInfo ()
		{
			var androidLocale = Java.Util.Locale.Default;

			//var netLanguage = androidLocale.Language.Replace ("_", "-");
			var netLanguage = androidLocale.ToString().Replace ("_", "-");

			//var netLanguage = androidLanguage.Replace ("_", "-");
			Console.WriteLine ("android:" + androidLocale.ToString());
			Console.WriteLine ("net:" + netLanguage);

			Console.WriteLine (Thread.CurrentThread.CurrentCulture);
			Console.WriteLine (Thread.CurrentThread.CurrentUICulture);

			return new System.Globalization.CultureInfo(netLanguage);
		}
	}
}

