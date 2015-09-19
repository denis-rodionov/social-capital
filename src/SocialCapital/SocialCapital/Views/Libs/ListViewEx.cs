using System;
using Xamarin.Forms;
using System.Windows.Input;

namespace SocialCapital.Views.Libs
{
	public class ListViewEx : ListView
	{
		public static BindableProperty ItemClickCommandProperty = BindableProperty.Create<ListViewEx, ICommand>(
			x => x.ItemClickCommand, 
			null);

		public static BindableProperty ItemClickCommandParameterProperty = BindableProperty.Create<ListViewEx, object>(
			x => x.ItemClickCommandParameter,
			null);


		public ListViewEx() {
			this.ItemTapped += this.OnItemTapped;
		}

		public ICommand ItemClickCommand {
			get { return (ICommand)this.GetValue(ItemClickCommandProperty); }
			set { this.SetValue(ItemClickCommandProperty, value); }
		}

		public object ItemClickCommandParameter {
			get { return this.GetValue (ItemClickCommandParameterProperty); }
			set { this.SetValue (ItemClickCommandParameterProperty, value); }
		}


		private void OnItemTapped(object sender, ItemTappedEventArgs e) {
			if (e.Item != null && this.ItemClickCommand != null && this.ItemClickCommand.CanExecute(e)) {
				this.ItemClickCommand.Execute(e.Item);
				this.SelectedItem = null;
			}
		}
	}
}

