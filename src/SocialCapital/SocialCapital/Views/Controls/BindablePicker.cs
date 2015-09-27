using System;

using Xamarin.Forms;
using System.Collections;

namespace SocialCapital.Views.Controls
{
	public class BindablePicker : Picker
	{
		public BindablePicker()
		{
			this.SelectedIndexChanged += OnSelectedIndexChanged;
		}

		public static BindableProperty ItemsSourceProperty =
			BindableProperty.Create<BindablePicker, IEnumerable>(o => o.ItemsSource, default(IEnumerable), propertyChanged: OnItemsSourceChanged);

		public static BindableProperty SelectedItemProperty =
			BindableProperty.Create<BindablePicker, object>(o => o.SelectedItem, default(object),propertyChanged: OnSelectedItemChanged);

		public static BindableProperty ConverterProperty =
			BindableProperty.Create<BindablePicker, IValueConverter>(o => o.Converter, default(IValueConverter), BindingMode.TwoWay, null,
				null, null, null); 

		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public object SelectedItem
		{
			get { return (object)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public IValueConverter Converter
		{
			get { return (IValueConverter)GetValue(ConverterProperty); }
			set
			{
				SetValue(ConverterProperty, value);
			}
		}

		private String Convert(object input, IValueConverter converter)
		{
			return converter == null 
				? input.ToString() 
					: (String)converter.Convert(input, typeof(String), null, System.Globalization.CultureInfo.CurrentUICulture);
		}

		private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
		{
			var picker = bindable as BindablePicker;
			picker.Items.Clear();
			if (newvalue != null)
			{
				//now it works like "subscribe once" but you can improve
				foreach (var item in newvalue)
				{
					picker.Items.Add (picker.Convert (item, picker.Converter));   //item.ToString());
				}
			}
		}


		private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
			{
				SelectedItem = null;
			}
			else
			{
				SelectedItem = Items[SelectedIndex];
			}
		}

		private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			var picker = bindable as BindablePicker;
			if (newvalue != null)
			{
				picker.SelectedIndex = picker.Items.IndexOf(newvalue.ToString());
			}
		}
	}
}


