using System;
using Xamarin.Forms;
using System.Dynamic;
using SocialCapital.ViewModels;
using System.Collections;
using SocialCapital.Data.Model.Enums;
using System.Collections.Generic;

namespace SocialCapital.Views.Controls
{
	public class ContactNativeCell : ViewCell
	{
		public ContactNativeCell ()
		{
		}

		/// <summary>
		/// Horrible piece of shit. But it need for event change handling. 
		/// </summary>
		/// <value></value>
		/// <remarks></remarks>
		public object View { get; set; }

		/// <summary>
		/// Full name of the contact
		/// </summary>
		public static readonly BindableProperty FullNameProperty = 
			BindableProperty.Create ("FullName", typeof(string), typeof(ContactNativeCell), "");
		public string FullName {
			get { return (string)GetValue (FullNameProperty); }
			set { SetValue (FullNameProperty, value); }
		}

		/// <summary>
		/// Color Status of the contact
		/// </summary>
		public static readonly BindableProperty ColorStatusProperty =
			BindableProperty.Create<ContactNativeCell, Color> (t => t.ColorStatus, Color.Silver);
		public Color ColorStatus {
			get { return (Color)GetValue (ColorStatusProperty); }
			set { SetValue (ColorStatusProperty, value); }
		}

		/// <summary>
		/// Contact image. Thumbnail.
		/// Null if empty
		/// </summary>
		public static readonly BindableProperty ContactImageProperty =
			BindableProperty.Create<ContactNativeCell, byte[]> (t => t.ContactImage, null);
		public byte[] ContactImage {
			get { return (byte[])GetValue (ContactImageProperty); }
			set { SetValue (ContactImageProperty, value); }
		}

		/// <summary>
		/// Group
		/// </summary>
		public static readonly BindableProperty GroupNameProperty =
			BindableProperty.Create<ContactNativeCell, string> (t => t.GroupName, null);
		public string GroupName {
			get { return (string)GetValue (GroupNameProperty); }
			set { SetValue (GroupNameProperty, value); }
		}

		/// <summary>
		/// Tags
		/// </summary>
		public static readonly BindableProperty TagsProperty =
			BindableProperty.Create<ContactNativeCell, IEnumerable<ILabel>> (t => t.Tags, null);
		public IEnumerable<ILabel> Tags {
			get { return (IEnumerable<ILabel>)GetValue (TagsProperty); }
			set { SetValue (TagsProperty, value); }
		}

		/// <summary>
		/// Icon
		/// </summary>
		public static readonly BindableProperty IconProperty =
			BindableProperty.Create<ContactNativeCell, string> (t => t.Icon, null);
		public string Icon {
			get { return (string)GetValue (IconProperty); }
			set { SetValue (IconProperty, value); }
		}

		/// <summary>
		/// Selected: null - no select checkbox.
		/// </summary>
		public static readonly BindableProperty SelectedProperty =
			BindableProperty.Create<ContactNativeCell, bool?> (t => t.Selected, null);
		public bool? Selected {
			get { return (bool?)GetValue (SelectedProperty); }
			set { SetValue (SelectedProperty, value); }
		}
	}
}


