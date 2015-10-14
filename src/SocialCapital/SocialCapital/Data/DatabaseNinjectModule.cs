using System;
using Ninject.Modules;
using SocialCapital.Data.Managers;

namespace SocialCapital.Data
{
	public class DatabaseNinjectModule : NinjectModule
	{
		public DatabaseNinjectModule ()
		{
		}

		#region implemented abstract members of NinjectModule

		public override void Load ()
		{
			// DataContext
			this.Bind<Func<IDataContext>>().ToMethod(ctx => () => new DataContext());

			// Managers
			this.Bind<FrequencyManager> ().To<FrequencyManager> ().InSingletonScope ();
			this.Bind<GroupsManager> ().To < GroupsManager> ().InSingletonScope ();
			this.Bind<ContactManager> ().To<ContactManager> ().InSingletonScope ();
			this.Bind<EmailManager> ().To<EmailManager> ().InSingletonScope ();
			this.Bind<PhonesManager> ().To<PhonesManager> ().InSingletonScope ();
			this.Bind<CommunicationManager> ().To<CommunicationManager> ().InSingletonScope ();
			this.Bind<ModificationManager> ().To<ModificationManager> ().InSingletonScope ();
			this.Bind<LogManager> ().To<LogManager> ().InSingletonScope ();
			this.Bind<ContactTagsManager> ().To<ContactTagsManager> ().InSingletonScope ();
			this.Bind<TagManager> ().To<TagManager> ().InSingletonScope ();

			this.Bind<Settings> ().ToSelf ();

			this.Bind<Migrator> ().ToSelf ().InSingletonScope ();

			this.Bind<DatabaseService> ().ToSelf ().InSingletonScope ();
		}

		#endregion
	}
}

