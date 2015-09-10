using System;

namespace SocialCapital.Common
{
	public class Timing
	{
		public DateTime StartTime { get; private set; }

		public string OperationName { get; private set; }

		private Timing() { }

		public static Timing Start(string operationName) {
			return new Timing () { 
				StartTime = DateTime.Now,
				OperationName = operationName
			};
		}

		public TimeSpan Finish(LogLevel level = LogLevel.Info)
		{
			var res = DateTime.Now - StartTime;
			Log.GetLogger ().Log (string.Format ("TIMING: operation '{0}' took {1:#.##} sec", OperationName, res.TotalSeconds));
		}
	}
}

