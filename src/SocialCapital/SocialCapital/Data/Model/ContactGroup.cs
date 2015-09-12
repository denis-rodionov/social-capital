using System;
using System.Collections.Generic;
using SocialCapital.Data.Model;

namespace SocialCapital.Data.Model
{
	/// <summary>
	/// Secondary view-model class for grouping list view
	/// </summary>
	public class ContactGroup<T> : IEnumerable<Contact>
	{
		public T GroupName { get; set; }
		public IEnumerable<Contact> Contacts { get; set; }

		#region IEnumerable implementation

		IEnumerator<Contact> IEnumerable<Contact>.GetEnumerator ()
		{
			return Contacts.GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return Contacts.GetEnumerator ();
		}

		#endregion
	}
}

