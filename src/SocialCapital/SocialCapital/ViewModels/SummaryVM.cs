using System;
using SocialCapital.Data.Managers;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Ninject;
using SocialCapital.Common.FormsMVVM;

namespace SocialCapital.ViewModels
{
	public class SummaryVM : ViewModelBase
	{
		#region Init

		readonly ContactManager contactManager;

		public SummaryVM (ContactManager contactManager)
		{
			this.contactManager = contactManager;
			Init ();
		}

		private void Init()
		{
			NotProcessedContacts = contactManager.GetContacts (c => c.GroupId == null).Count ();
			TotalContactsCount = contactManager.AllContacts .Count ();
			ProcessedContacts = TotalContactsCount - NotProcessedContacts;
			DeletedCount = contactManager.GetDeleted ().Count();
		}

		#endregion

		#region Property

		public int NotProcessedContacts { get; set; }

		public int ProcessedContacts { get; set; }

		public int TotalContactsCount { get; set; }

		private int deletedCount;
		public int DeletedCount { 
			get { return deletedCount; } 
			set { SetProperty (ref deletedCount, value); } 
		}

		#endregion

		#region actions

		public ICommand ShowDeletedList { get { return new Command (ShowDeletedListExecute); } }
		private void ShowDeletedListExecute()
		{
			var navigator = App.Container.Get<INavigator> ();

			navigator.PushAsync<DeleteContactsVM> (vm => {
				vm.PropertyChanged += (s, e) => { 
					contactManager.GetDeleted ().Count();
				};
			});
		}

		#endregion
	}
}

