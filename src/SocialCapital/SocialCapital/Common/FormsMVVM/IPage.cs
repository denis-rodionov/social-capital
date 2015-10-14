using System;
using SocialCapital.Common.FormsMVVM;
using Xamarin.Forms;

namespace SocialCapital.Common.FormsMVVM
{
	public interface IPage : IDialogProvider
	{
		INavigation Navigation { get; }
	}
}

