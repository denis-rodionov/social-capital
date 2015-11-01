using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using System.Resources;
using System.Globalization;
using System.Reflection;

namespace SocialCapital.Common
{
	// You exclude the 'Extension' suffix when using in Xaml markup
	[ContentProperty ("Text")]
	public class TranslateExtension : IMarkupExtension
	{
		static CultureInfo ci;
		const string ResourceId = "SocialCapital.AppResources";

		public TranslateExtension() {
			if (ci == null)
				ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo ();
		}

		public string Text { get; set; }

		public string Format { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			if (Text == null)
				return "";

			ResourceManager temp = new ResourceManager(ResourceId
				, typeof(TranslateExtension).GetTypeInfo().Assembly);

			var translation = temp.GetString (Text, ci);

			if (translation != null)
			{
				if (Format != null)
					translation = string.Format(Format, translation);
			}
			else 	// translation not found
			{
				#if DEBUG
				throw new ArgumentException (
					String.Format ("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
					"Text");
				#else
				translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
				#endif
			}
			return translation;
		}
	}
}

