using System;
using System.ComponentModel;

namespace SocialCapital.Common.FormsMVVM
{
	public interface IViewModel : INotifyPropertyChanged
	{
		string Title { get; set; }

		void SetState<T>(Action<T> action) where T : class, IViewModel;
	}
}

