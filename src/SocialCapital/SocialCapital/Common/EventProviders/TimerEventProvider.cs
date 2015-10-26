using System;
using Xamarin.Forms;
using SocialCapital.Data;

namespace SocialCapital.Common.EventProviders
{
	public class TimerEventProvider : IEventProvider
	{
		readonly TimeSpan StdInterval = TimeSpan.FromMinutes(1);

		readonly TimeSpan interval;

		public bool IsEnabled = false;
		public DateTime LastEventTime;

		public TimerEventProvider (TimeSpan interval, DateTime lastEventTime)
		{
			this.interval = interval;

			if (lastEventTime != null)
				this.LastEventTime = DateTime.Now;
		}

		#region IEventProvider implementation

		public event Action Raised;

		public void Start ()
		{
			IsEnabled = true;
			StartTimer ();
		}

		public void Stop ()
		{
			IsEnabled = false;
		}

		#endregion

		#region Implementation

		private void RaiseEvent()
		{
			LastEventTime = DateTime.Now;
			var handle = Raised;
			if (handle != null)
				Raised ();
		}

		private bool Callback()
		{
			try
			{
			if (IsEnabled)
			{
				if (DateTime.Now - LastEventTime > interval)
					RaiseEvent ();

				return true;
			} else
			{
				deviceTimerStarted = false;
				return false;
			}
			}
			catch (Exception ex)
			{
				throw new EventProviderException ("Exception in event consumers", ex);
			}
		}

		private bool deviceTimerStarted = false;
		private object guard = new object();
		private void StartTimer()
		{
			lock (guard)
			{
				if (!deviceTimerStarted)
				{
					deviceTimerStarted = true;

					var concreetInterval = interval > StdInterval ? StdInterval : interval;

					Device.StartTimer (concreetInterval, Callback);
				}
			}
		}

		#endregion
	}
}

