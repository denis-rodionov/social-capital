﻿using System;

using Xamarin.Forms;
using Ninject.Modules;

namespace SocialCapital.ViewModels
{
	public class ViewModelNinjectModule : NinjectModule
	{
		#region implemented abstract members of NinjectModule
		public override void Load ()
		{
			this.Bind<ContactListVM> ().To<ContactListVM> ();
		}
		#endregion
	}
}


