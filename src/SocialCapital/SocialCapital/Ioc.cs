using System;
using Microsoft.Practices.Unity;
using SocialCapital.Data;

namespace SocialCapital
{
	public class Ioc
	{
		public static UnityContainer Container { get; set; }

		public Ioc ()
		{
			Ioc.Container = new UnityContainer ();
		}
	}
}

