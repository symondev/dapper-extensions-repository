using System;
using System.Data.SqlClient;
using Dapper.Extensions.Repository.Context;
using Microsoft.Extensions.Logging.Debug;

namespace Dapper.Extensions.Repository.Tests.Fixture
{
    public class MsSqlDatabaseFixture : IDisposable
    {
        public MsSqlDatabaseFixture()
        {
            var connString = "server=192.168.0.117;database=Dapper.Extensions.Repository;uid=sa;pwd=Ls1;";
            //var connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Dapper.Repository;Integrated Security=True;";

            Db = new DbContext(new SqlConnection(connString), new DebugLogger("TestLogger"));

            InitDb();
        }

        public DbContext Db { get; }

        private void InitDb()
        {
            Action<string> dropTable = name => Db.Connection.Execute($@"IF OBJECT_ID('{name}', 'U') IS NOT NULL DROP TABLE [{name}]; ");
            dropTable("Users");
            dropTable("Cars");
            dropTable("CarOptions");
            dropTable("Roles");
            dropTable("Images");
            dropTable("Dishes");
            dropTable("DishImages");
            dropTable("DishOptions");
            dropTable("RecordsForInsert");
            dropTable("RecordsForInsertAsync");
            dropTable("RecordsForUpdate");
            dropTable("RecordsForDelete");

            Db.Connection.Execute(@"CREATE TABLE Users (Id int IDENTITY(1,1) not null, Name varchar(50) not null, Deleted bit not null, PRIMARY KEY (Id))");
            Db.Connection.Execute(@"CREATE TABLE Cars (Id int IDENTITY(1,1) not null, CarName varchar(50) not null, UserId int, PRIMARY KEY (Id))");
            Db.Connection.Execute(@"CREATE TABLE CarOptions (Id int IDENTITY(1,1) not null, OptionName varchar(50) not null, CarId int, PRIMARY KEY (Id))");
            Db.Connection.Execute(@"CREATE TABLE Roles (Id int IDENTITY(1,1) not null, Name varchar(50) not null, UserId int, PRIMARY KEY (Id))");
            Db.Connection.Execute(@"CREATE TABLE Images (Id int IDENTITY(1,1) not null, Name varchar(50) not null, UserId int, PRIMARY KEY (Id))");
            Db.Connection.Execute(@"CREATE TABLE Dishes (DishId int IDENTITY(1,1) not null, Name varchar(50) not null, PRIMARY KEY (DishId))");
            Db.Connection.Execute(@"CREATE TABLE DishImages (DishImageId int IDENTITY(1,1) not null, DishId int not null, Image varchar(50) not null, PRIMARY KEY (DishImageId))");
            Db.Connection.Execute(@"CREATE TABLE DishOptions (DishOptionId int IDENTITY(1,1) not null, DishId int not null, [Option] varchar(50) not null, PRIMARY KEY (DishOptionId))");

            Db.Connection.Execute(@"CREATE TABLE RecordsForInsert (Id int IDENTITY(1,1) not null, [Name] varchar(50) not null, PRIMARY KEY (Id))");
            Db.Connection.Execute(@"CREATE TABLE RecordsForInsertAsync (Id int IDENTITY(1,1) not null, [Name] varchar(50) not null, PRIMARY KEY (Id))");
            Db.Connection.Execute(@"CREATE TABLE RecordsForUpdate (Id int IDENTITY(1,1) not null, [Name] varchar(50) not null, PRIMARY KEY (Id))");
            Db.Connection.Execute(@"CREATE TABLE RecordsForDelete (Id int IDENTITY(1,1) not null, [Name] varchar(50) not null, PRIMARY KEY (Id))");


            Db.Connection.Execute($"INSERT INTO [dbo].[Users]([Name],[Deleted])VALUES('Name1', 0)");
            Db.Connection.Execute($"INSERT INTO [dbo].[Users]([Name],[Deleted])VALUES('Name2', 0)");
            Db.Connection.Execute($"INSERT INTO [dbo].[Users]([Name],[Deleted])VALUES('Name3', 1)");
            Db.Connection.Execute($"INSERT INTO [dbo].[Users]([Name],[Deleted])VALUES('Name4', 0)");
            Db.Connection.Execute($"INSERT INTO [dbo].[Cars]([CarName],[UserId])VALUES('Car1', 1)");
            Db.Connection.Execute($"INSERT INTO [dbo].[Cars]([CarName],[UserId])VALUES('Car2', 1)");
            Db.Connection.Execute($"INSERT INTO [dbo].[CarOptions]([OptionName],[CarId])VALUES('Option1', 1)");
            Db.Connection.Execute($"INSERT INTO [dbo].[CarOptions]([OptionName],[CarId])VALUES('Option2', 2)");
            Db.Connection.Execute($"INSERT INTO [dbo].[Roles]([Name],[UserId])VALUES('Role1', 1)");
            Db.Connection.Execute($"INSERT INTO [dbo].[Roles]([Name],[UserId])VALUES('Role2', 1)");
            Db.Connection.Execute($"INSERT INTO [dbo].[Roles]([Name],[UserId])VALUES('Role3', 1)");
            Db.Connection.Execute($"INSERT INTO [dbo].[Roles]([Name],[UserId])VALUES('Role1', 2)");
            Db.Connection.Execute($"INSERT INTO [dbo].[Images]([Name],[UserId])VALUES('Image1', 1)");
            Db.Connection.Execute($"INSERT INTO [dbo].[Images]([Name],[UserId])VALUES('Image4', 4)");

            Db.Connection.Execute($"INSERT INTO [dbo].[Dishes]([Name])VALUES('Dish1')");
            Db.Connection.Execute($"INSERT INTO [dbo].[Dishes]([Name])VALUES('Dish2')");
            Db.Connection.Execute($"INSERT INTO [dbo].[Dishes]([Name])VALUES('Dish3')");
            Db.Connection.Execute($"INSERT INTO [dbo].[DishImages]([Image],[DishId])VALUES('DishImage1', 1)");
            Db.Connection.Execute($"INSERT INTO [dbo].[DishImages]([Image],[DishId])VALUES('DishImage2', 2)");
            Db.Connection.Execute($"INSERT INTO [dbo].[DishImages]([Image],[DishId])VALUES('DishImage1', 3)");
            Db.Connection.Execute($"INSERT INTO [dbo].[DishImages]([Image],[DishId])VALUES('DishImage2', 3)");
            Db.Connection.Execute($"INSERT INTO [dbo].[DishOptions]([Option],[DishId])VALUES('DishOption1', 1)");
            Db.Connection.Execute($"INSERT INTO [dbo].[DishOptions]([Option],[DishId])VALUES('DishOption2', 1)");
            Db.Connection.Execute($"INSERT INTO [dbo].[DishOptions]([Option],[DishId])VALUES('DishOption1', 3)");

            Db.Connection.Execute($"INSERT INTO [dbo].[RecordsForUpdate]([Name])VALUES('1')");
            Db.Connection.Execute($"INSERT INTO [dbo].[RecordsForUpdate]([Name])VALUES('2')");

            Db.Connection.Execute($"INSERT INTO [dbo].[RecordsForDelete]([Name])VALUES('1')");
            Db.Connection.Execute($"INSERT INTO [dbo].[RecordsForDelete]([Name])VALUES('2')");
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}
