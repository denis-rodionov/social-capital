using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SocialCapital.Common.FormsMVVM
{
	public class Navigator : INavigator
	{
		private readonly Func<INavigation> _navigationFactory;
		private readonly IViewFactory _viewFactory;

		public Navigator(Func<INavigation> navigationFactory, IViewFactory viewFactory)
		{
			_navigationFactory = navigationFactory;
			_viewFactory = viewFactory;
		}

		private INavigation Navigation
		{
			get { return _navigationFactory(); }
		}

		public async Task<IViewModel> PopAsync()
		{
			Page view = await Navigation.PopAsync();
			return view.BindingContext as IViewModel;
		}

		public async Task<IViewModel> PopModalAsync()
		{
			Page view = await Navigation.PopAsync();
			return view.BindingContext as IViewModel;
		}

		public async Task PopToRootAsync()
		{
			await Navigation.PopToRootAsync();
		}

		public async Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel
		{
			TViewModel viewModel;
			var view = _viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
			await Navigation.PushAsync(view);
			return viewModel;
		}

		public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel) 
			where TViewModel : class, IViewModel
		{
			var view = _viewFactory.Resolve(viewModel);
			await Navigation.PushAsync(view);
			return viewModel;
		}

		public async Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel
		{
			TViewModel viewModel;
			var view = _viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
			await Navigation.PushModalAsync(view);
			return viewModel;
		}

		public async Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel) 
			where TViewModel : class, IViewModel
		{
			var view = _viewFactory.Resolve(viewModel);
			await Navigation.PushModalAsync(view);
			return viewModel;
		}
	}
}

