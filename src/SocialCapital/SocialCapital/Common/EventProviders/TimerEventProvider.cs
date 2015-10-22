using System;
using Xamarin.Forms;

namespace SocialCapital.Common.EventProviders
{
	public class TimerEventProvider : IEventProvider
	{
		public TimeSpan Interval;

		public bool IsEnabled = false;

		public TimerEventProvider (TimeSpan interval)
		{
			Interval = interval;
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

		private bool Callback()
		{
			if (IsEnabled)
			{
				var handle = Raised;
				if (handle != null)
					Raised ();

				return true;
			}
			else
				return false;
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
					Device.StartTimer (Interval, Callback);
				}
			}
		}

		#endregion
	}
}

