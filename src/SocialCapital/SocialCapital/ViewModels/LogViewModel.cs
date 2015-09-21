using System;
using SocialCapital.Logging;
using System.Collections.Generic;
using SocialCapital.Data;
using SocialCapital.Data.Model;
using System.Linq;

namespace SocialCapital.ViewModels
{
	public class LogViewModel : ViewModelBase
	{
		public LogViewModel()
		{
			logs = DataContext.GetLogs ().OrderByDescending (l => l.Time);
		}

		/// <summary>
		/// String for filtering contact list
		/// </summary>
		string filter = "";
		public string Filter {
			get { return filter; }
			set { 
				SetProperty (ref filter, value); 
				OnPropertyChanged ("Logs");
			}
		}

		private IEnumerable<LogMessage> logs;
		public IEnumerable<LogMessage> Logs 
		{
			get { return logs.Where (l => l.Message.ToLower ().Contains (filter.ToLower ())); }
		}
	}
}

