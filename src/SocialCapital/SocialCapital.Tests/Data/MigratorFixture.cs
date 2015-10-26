using System;
using NUnit.Framework;
using SocialCapital.Data;
using SocialCapital.Tests.Data.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace SocialCapital.Tests.Data
{
	[TestFixture]
	public class MigratorFixture
	{
		[Test]
		public void DoNotMigrateSameVersionTest()
		{
			// arrange
			var settings = new SettingsMock ();
			settings.Dict.Add (Migrator.DatabaseVersionConfig, "1.4");

			var migrations = new List<MigrationMock> () {
				new MigrationMock("0.1"),
				new MigrationMock("2.0"), 
				new MigrationMock("0.5")
			};
			var migrator = new Migrator (settings, "1.4", migrations.Cast<IMigration>());

			// act
			migrator.Migrate(new DataContextMock());

			// assert
			Assert.IsFalse(((MigrationMock)migrations[0]).Done, "Do not migrate if device version (in database) == code version (DatabaseService.DatabaseVersion)");
			Assert.IsFalse(migrations[1].Done, "Do not migrate if device version (in database) == code version (DatabaseService.DatabaseVersion)");
			Assert.IsFalse(migrations[2].Done, "Do not migrate if device version (in database) == code version (DatabaseService.DatabaseVersion)");
		}

		[Test]
		public void SameMigrationVersionTest()
		{
			// arrange
			var settings = new SettingsMock ();
			settings.Dict.Add (Migrator.DatabaseVersionConfig, "1.4");

			var migrations = new List<MigrationMock> () {
				new MigrationMock("1.4")
			};
			var migrator = new Migrator (settings, "1.5", migrations.Cast<IMigration>()) ;

			// act
			migrator.Migrate(new DataContextMock());

			// assert
			Assert.IsFalse(migrations.Single().Done, "If migration version equals the device version - DO NOT MIGRATE");
		}

		[Test]
		public void LessMigrationVersion()
		{
			// arrange
			var settings = new SettingsMock ();
			settings.Dict.Add (Migrator.DatabaseVersionConfig, "1.3");

			var migrations = new List<MigrationMock> () {
				new MigrationMock("0.3")
			};
			var migrator = new Migrator (settings, "2.5", migrations.Cast<IMigration>()) ;

			// act
			migrator.Migrate(new DataContextMock());

			// assert
			Assert.IsFalse(migrations.Single().Done, "If migration version less then device verstion: do not migrate");
		}

		[Test]
		public void LessMigrationVersion_1()
		{
			// arrange
			var settings = new SettingsMock ();
			settings.Dict.Add (Migrator.DatabaseVersionConfig, "1.3");

			var migrations = new List<MigrationMock> () {
				new MigrationMock("1.2")
			};
			var migrator = new Migrator (settings, "2.5", migrations.Cast<IMigration>()) ;

			// act
			migrator.Migrate(new DataContextMock());

			// assert
			Assert.IsFalse(migrations.Single().Done, "If migration version less then device verstion: do not migrate");
		}

		[Test]
		public void GreaterMigrationVersionTest()
		{
			// arrange
			var settings = new SettingsMock ();
			settings.SaveValue (Migrator.DatabaseVersionConfig, "2.3");

			var migrations = new List<MigrationMock> () {
				new MigrationMock("2.4")
			};
			var migrator = new Migrator (settings, "2.5", migrations.Cast<IMigration>()) ;

			// act
			migrator.Migrate(new DataContextMock());

			// assert
			Assert.IsTrue(migrations.Single().Done, "If migration version greater then device version: migrate");
		}

		[Test]
		public void GreaterMigrationVersionTest1()
		{
			// arrange
			var settings = new SettingsMock ();
			settings.Dict.Add (Migrator.DatabaseVersionConfig, "1.5");

			var migrations = new List<MigrationMock> () {
				new MigrationMock("2.4")
			};
			var migrator = new Migrator (settings, "2.5", migrations.Cast<IMigration>()) ;

			// act
			migrator.Migrate(new DataContextMock());

			// assert
			Assert.IsTrue(migrations.Single().Done, "If migration version greater then device version: migrate");
		}
	}
}

