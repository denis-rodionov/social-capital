using System;
using Ninject.Modules;

namespace SocialCapital.Views
{
	public class ViewNInjectModule : NinjectModule
	{
		public ViewNInjectModule ()
		{
		}

		#region implemented abstract members of NinjectModule

		public override void Load ()
		{
			Bind<SettingsPage> ().ToSelf ();
			Bind<SummaryPage> ().ToSelf ();
		}

		#endregion
	}
}

