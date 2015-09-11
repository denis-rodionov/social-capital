using System;
using SQLite.Net.Attributes;

namespace SocialCapital.Data.Model
{
	public class Config
	{
		[PrimaryKey, AutoIncrement]
		public string Key { get; set; }

		public string Value { get; set; }

		public int GetIntValue()
		{
			if (Value == null)
				throw new Exception ("Value is null");

			int parseRes;
			var canParse = int.TryParse (Value, out parseRes);

			if (!canParse)
				throw new Exception (string.Format ("Cannot get int value from string '{0}'", Value));
					
			return parseRes;
		}

		public long GetLongValue()
		{
			if (Value == null)
				throw new Exception ("Value is null");

			long parseRes;
			var canParse = long.TryParse (Value, out parseRes);

			if (!canParse)
				throw new Exception (string.Format ("Cannot get int value from string '{0}'", Value));

			return parseRes;
		}

		public void SetValue(int value)
		{
			Value = value.ToString ();
		}

		public DateTime GetDateTimeValue()
		{
			return new DateTime (GetIntValue ());
		}

		public void SetValue(DateTime value)
		{
			Value = value.Ticks.ToString ();
		}

		public void SetValue<T>(T value)
		{
			if (typeof(T) == typeof(int))
				SetValue (value);
			else if (typeof(T) == typeof(string))
				Value = (value as object).ToString();
			else if (typeof(T) == typeof(DateTime))
				SetValue (value);
			else
				throw new Exception ("Unknow type of the argument");
		}

		public T GetValue<T>() 
		{
			if (typeof(T) == typeof(int))
				return (T)((object)Convert.ToInt32 (GetIntValue ()));
			else if (typeof(T) == typeof(string))
				return (T)((object)Convert.ToString(Value));
			else if (typeof(T) == typeof(DateTime))
				return (T)((object)Convert.ToDateTime(GetDateTimeValue ()));
			else
				throw new Exception ("Unknow type of the argument");
		}
	}
}

