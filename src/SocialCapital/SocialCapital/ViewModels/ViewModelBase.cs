using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SocialCapital.Data;
using Ninject;

namespace SocialCapital.ViewModels
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

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

		protected ContactManager Database { get { return App.Container.Get<ContactManager> (); 	}}

	}
}

