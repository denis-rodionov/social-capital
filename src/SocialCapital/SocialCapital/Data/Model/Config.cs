﻿using System;
using SQLite.Net.Attributes;

namespace SocialCapital.Data.Model
{
	public class Config
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Unique]
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
				throw new Exception (string.Format ("Cannot get long value from string '{0}'", Value));

			return parseRes;
		}

		public bool GetBoolValue()
		{
			if (Value == null)
				throw new Exception ("Value is null");

			bool parseRes;
			var canParse = bool.TryParse (Value, out parseRes);

			if (!canParse)
				throw new Exception (string.Format ("Cannot get bool value from string '{0}'", Value));

			return parseRes;
		}

		public void SetValue(int value)
		{
			Value = value.ToString ();
		}

		public void SetValue(long value)
		{
			Value = value.ToString ();
		}

		public void SetValue(bool value)
		{
			Value = value.ToString ();
		}

		public DateTime GetDateTimeValue()
		{
			return new DateTime (GetLongValue ());
		}

		public void SetDateTimeValue(DateTime value)
		{
			Value = value.Ticks.ToString ();
		}

		public void SetValue<T>(T value) 
		{
			if (typeof(T) == typeof(int))
				SetValue (Convert.ToInt32 (value));
			else if (typeof(T) == typeof(long) || typeof(T) == typeof(long?))
				SetValue (Convert.ToInt64 (value));
			else if (typeof(T) == typeof(string))
				Value = (value as object).ToString ();
			else if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
				SetDateTimeValue (Convert.ToDateTime (value));
			else if (typeof(T) == typeof(bool))
				SetValue (Convert.ToBoolean (value));
			else
				throw new Exception (string.Format ("MyError: Unknow type of the argument '{0}'", value));
		}

		public T GetValue<T>() 
		{
			if (typeof(T) == typeof(int))
				return (T)((object)Convert.ToInt32 (GetIntValue ()));
			else if (typeof(T) == typeof(long) || typeof(T) == typeof(long?))
				return (T)((object)Convert.ToInt64 (GetLongValue ()));
			else if (typeof(T) == typeof(string))
				return (T)((object)Convert.ToString (Value));
			else if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
				return (T)((object)Convert.ToDateTime (GetDateTimeValue ()));
			else if (typeof(T) == typeof(bool))
				return (T)((object)Convert.ToBoolean (GetBoolValue ()));
			else
				throw new Exception (string.Format("Unknow type of the argument '{0}'", typeof(T)));
		}
	}
}

