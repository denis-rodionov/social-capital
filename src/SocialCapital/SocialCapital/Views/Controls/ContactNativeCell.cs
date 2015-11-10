using System;
using Xamarin.Forms;
using System.Dynamic;

namespace SocialCapital.Views.Controls
{
	public class ContactNativeCell : ViewCell
	{
		public ContactNativeCell ()
		{
		}

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
			BindableProperty.Create<ContactNativeCell, ImageSource> (t => t.ContactImage, null);
		public ImageSource ContactImage {
			get { return (ImageSource)GetValue (ContactImageProperty); }
			set { SetValue (ContactImageProperty, value); }
		}
	}
}


