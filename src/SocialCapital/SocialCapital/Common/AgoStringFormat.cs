using System;

namespace SocialCapital
{
	public static class AgoStringFormat
	{
		public static string ToAgoFormatRus (this DateTime time)
		{
			// 1.
			// Get time span elapsed since the date.
			TimeSpan s = DateTime.Now.Subtract(time);

			// 2.
			// Get total number of days elapsed.
			int dayDiff = (int)s.TotalDays;

			int minDiff = (int)s.TotalMinutes;

			int hourDiff = (int)s.TotalHours;

			int weelDiff = dayDiff / 7;

			// 3.
			// Get total number of seconds elapsed.
			int secDiff = (int)s.TotalSeconds;

			// 4.
			// Don't allow out of range values.
			if (dayDiff < 0 || dayDiff >= 31)
			{
				return null;
			}

			// 5.
			// Handle same-day times.
			if (dayDiff == 0)
			{
				// A.
				// Less than one minute ago.
				if (secDiff < 60)
				{
					return "только что";
				}
				// B.
                               				// Less than 2 minutes ago.
				if (hourDiff < 1)
				{
					if (minDiff % 10 == 1 && minDiff != 11)
						return minDiff + " минуту назад";
					else if (minDiff % 10 < 2 && minDiff % 10 < 5 && minDiff != 12)
						return minDiff + " минуты назад";
					else
						return minDiff + " минут назад";
				}

				// D.
				// Less than 2 hours ago.
				if (hourDiff == 1 || hourDiff == 21)
					return hourDiff + " час";
				else if (hourDiff == 2 || hourDiff == 3 || hourDiff == 4 ||
				         hourDiff == 22 || hourDiff == 23)
					return hourDiff + " часа";
				else
					return hourDiff + " часов";
			}

			// 6.
			// Handle previous days.
			if (dayDiff == 1)
			{
				return "вчера";
			} else if (dayDiff < 5)
			{
				return string.Format ("{0} дня назад",
					dayDiff);
			} else if (dayDiff < 7)
				return dayDiff + " дней назад";
			
			if (dayDiff < 31)
			{
				if (weelDiff == 1)
					return weelDiff + " неделю назад";
				else
					return weelDiff + " недели назад";
			}

			return time.ToString("D");
		}
	}
}

