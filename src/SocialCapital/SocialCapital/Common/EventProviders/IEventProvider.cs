using System;

namespace SocialCapital.Common.EventProviders
{
	public interface IEventProvider
	{
		event Action Raised;

		void Start();

		void Stop();
	}
}

