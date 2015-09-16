using System;
using System.Collections.Generic;
using SocialCapital.Data.Model;

namespace SocialCapital.ViewModels
{
	/// <summary>
	/// Secondary view-model class for grouping list view
	/// </summary>
	public class GroupVM<GroupType, ElementType> : IEnumerable<ElementType>
	{
		public GroupType Group { get; set; }
		public List<ElementType> Elements { get; set; }

		#region IEnumerable implementation

		IEnumerator<ElementType> IEnumerable<ElementType>.GetEnumerator ()
		{
			return Elements.GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return Elements.GetEnumerator ();
		}

		#endregion
	}
}

