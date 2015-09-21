using System;
using System.Collections.Generic;

namespace SocialCapital.Common
{
	public class TimingStatiestics {
		public int LaunchCount { get; set; }
		public TimeSpan AverageDuration { get; set; }
	}

	public class Timing
	{
		public static Dictionary<string, TimingStatiestics> Statistics;

		public DateTime StartTime { get; private set; }

		public string OperationName { get; private set; }

		public TimeSpan OperationTime { get; private set; }

		private Timing() 
		{
			if (Statistics == null)
				Statistics = new Dictionary<string, TimingStatiestics> ();
		}

		public static Timing Start(string operationName) {
			return new Timing () { 
				StartTime = DateTime.Now,
				OperationName = operationName
			};
		}

		public TimeSpan Finish(LogLevel level = LogLevel.Info)
		{
			OperationTime = DateTime.Now - StartTime;
			SetStatistics ();
			Log.GetLogger ().Log (string.Format ("TIMING: operation '{0}' took {1:#.##} sec", OperationName, OperationTime.TotalSeconds));
			return OperationTime;
		}

		private void SetStatistics()
		{
			if (!Statistics.ContainsKey (OperationName))
				Statistics.Add (OperationName, new TimingStatiestics () {
					LaunchCount = 0,
					AverageDuration = TimeSpan.FromTicks (0)
				});
			
			var	stat = Statistics [OperationName];

			stat.AverageDuration = TimeSpan.FromTicks (
				(TimeSpan.FromTicks (stat.AverageDuration.Ticks * stat.LaunchCount) + OperationTime).Ticks
				/ (stat.LaunchCount + 1)
			);

			stat.LaunchCount++;
		}
	}
}

