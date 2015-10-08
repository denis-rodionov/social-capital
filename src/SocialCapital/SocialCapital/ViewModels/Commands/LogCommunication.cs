using System;
using System.Windows.Input;
using Xamarin.Forms;
using SocialCapital.Data.Managers;
using Ninject;
using SocialCapital.Data.Model.Enums;
using SocialCapital.Data.Model;

namespace SocialCapital.ViewModels.Commands
{
	public class LogCommunication : ICommand
	{
		private Contact SourceContact { get; set; }

		public LogCommunication (Contact contact)
		{
			SourceContact = contact;
		}

		#region ICommand implementation

		public event Action CommandExecuted;
		public event EventHandler CanExecuteChanged;

		public bool CanExecute (object parameter)
		{
			return true;
		}

		public void Execute (object parameter)
		{
			var page = (Page)parameter;

			App.Container.Get<CommunicationManager> ().SaveNewCommunication (
				new SocialCapital.Data.Model.CommunicationHistory() {
					ContactId = SourceContact.Id,
					Time = DateTime.Now,
					Type = CommunicationType.Unknown
				}
			);

			RaiceExecuted ();
		}

		private void RaiceExecuted()
		{
			var handler = CommandExecuted;
			if (handler != null)
				handler ();
		}

		#endregion
	}
}

