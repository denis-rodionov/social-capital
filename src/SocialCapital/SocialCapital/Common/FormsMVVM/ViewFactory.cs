using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Ninject;
using Ninject.Syntax;

namespace SocialCapital.Common.FormsMVVM
{
	public class ViewFactory : IViewFactory
	{
		private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();
		private readonly IResolutionRoot _componentContext;

		public ViewFactory(IResolutionRoot componentContext)
		{
			_componentContext = componentContext;
		}

		public void Register<TViewModel, TView>() 
			where TViewModel : class, IViewModel 
			where TView : Page
		{
			_map[typeof(TViewModel)] = typeof(TView);
		}

		public Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null) where TViewModel : class, IViewModel
		{
			TViewModel viewModel;
			return Resolve<TViewModel>(out viewModel, setStateAction);
		}

		public Page Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel 
		{
			viewModel = _componentContext.Get<TViewModel>();

			var viewType = _map[typeof(TViewModel)];
			var view = _componentContext.Get(viewType) as Page;

			if (setStateAction != null)
				viewModel.SetState(setStateAction);

			view.BindingContext = viewModel;
			return view;
		}

		public Page Resolve<TViewModel>(TViewModel viewModel) 
			where TViewModel : class, IViewModel 
		{
			var viewType = _map[typeof(TViewModel)];
			var view = _componentContext.Get(viewType) as Page;
			view.BindingContext = viewModel;
			return view;
		}
	}
}

