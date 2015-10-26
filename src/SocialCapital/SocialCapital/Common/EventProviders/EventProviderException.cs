using System;

namespace SocialCapital.Common.EventProviders
{
	public class EventProviderException : Exception
	{
		public EventProviderException (string message, Exception inner) : base(message, inner)
		{
		}
	}
}

