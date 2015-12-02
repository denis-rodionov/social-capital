using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SocialCapital.Data;
using Ninject;
using SocialCapital.Data.Managers;
using SocialCapital.Common.FormsMVVM;

namespace SocialCapital.ViewModels
{
	public class ViewModelBase : IViewModel
	{
		public string Title { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public void SetState<T>(Action<T> action) where T : class, IViewModel
		{
			action(this as T);
		}

		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals (storage, value))
				return false;

			storage = value;
			OnPropertyChanged (propertyName);
			return true;
		}

		protected void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}



	}
}

