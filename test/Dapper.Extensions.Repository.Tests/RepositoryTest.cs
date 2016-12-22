using System.Linq;
using System.Threading.Tasks;
using Dapper.Extensions.Repository.Tests.Entities;
using Dapper.Extensions.Repository.Tests.Fixture;
using Xunit;

namespace Dapper.Extensions.Repository.Tests
{
    public class RepositoryTest : IClassFixture<MsSqlDatabaseFixture>
    {
        private readonly MsSqlDatabaseFixture _fixture;

        public RepositoryTest(MsSqlDatabaseFixture msFixture)
        {
            _fixture = msFixture;
        }

        #region Insert

        [Fact]
        public async Task TestInsertAsync()
        {
            var insertRecord1 = new RecordsForInsertAsync {Name = "1"};
            var result = await _fixture.Db.SetEntity<RecordsForInsertAsync>().InsertAsync(insertRecord1);
            Assert.True(result);
            Assert.True(insertRecord1.Id == 1);

            var insertRecord2 = new RecordsForInsertAsync {Name = "2"};
            result = await _fixture.Db.SetEntity<RecordsForInsertAsync>().InsertAsync(insertRecord2);
            Assert.True(result);
            Assert.True(insertRecord2.Id == 2);
        }

        [Fact]
        public void TestInsert()
        {
            var insertRecord1 = new RecordsForInsert {Name = "1"};
            var result = _fixture.Db.SetEntity<RecordsForInsert>().Insert(insertRecord1);
            Assert.True(result);
            Assert.True(insertRecord1.Id == 1);

            var insertRecord2 = new RecordsForInsert {Name = "2"};
            result = _fixture.Db.SetEntity<RecordsForInsert>().Insert(insertRecord2);
            Assert.True(result);
            Assert.True(insertRecord2.Id == 2);
        }

        #endregion

        #region Update

        [Fact]
        public async Task TestUpdateAsync()
        {
            var record = await _fixture.Db.SetEntity<RecordsForUpdate>().FindAsync(p => p.Id == 1);
            Assert.True(record.Name == "1");

            record.Name = "10";
            var result = await _fixture.Db.SetEntity<RecordsForUpdate>().UpdateAsync(record);
            Assert.True(result);

            record = await _fixture.Db.SetEntity<RecordsForUpdate>().FindAsync(p => p.Id == 1);
            Assert.True(record.Name == "10");
        }

        [Fact]
        public void TestUpdate()
        {
            var record = _fixture.Db.SetEntity<RecordsForUpdate>().Find(p => p.Id == 2);
            Assert.True(record.Name == "2");

            record.Name = "20";
            var result = _fixture.Db.SetEntity<RecordsForUpdate>().Update(record);
            Assert.True(result);

            record = _fixture.Db.SetEntity<RecordsForUpdate>().Find(p => p.Id == 2);
            Assert.True(record.Name == "20");
        }

        #endregion

        #region Delete

        [Fact]
        public async Task TestDeleteAsync()
        {
            var result = await _fixture.Db.SetEntity<RecordsForDelete>().DeleteAsync(new RecordsForDelete {Id = 1});
            Assert.True(result);

            var record = await _fixture.Db.SetEntity<RecordsForDelete>().FindAsync(p => p.Id == 1);
            Assert.True(record == null);
        }

        [Fact]
        public void TestDelete()
        {
            var result = _fixture.Db.SetEntity<RecordsForDelete>().Delete(new RecordsForDelete { Id = 2 });
            Assert.True(result);

            var record = _fixture.Db.SetEntity<RecordsForDelete>().Find(p => p.Id == 2);
            Assert.True(record == null);
        }

        #endregion

        #region Count

        [Fact]
        public void TestCount()
        {
            var count = _fixture.Db.SetEntity<User>().Count();
            Assert.Equal(3, count);

            count = _fixture.Db.SetEntity<Role>().Count(p => p.UserId == 1);
            Assert.Equal(3, count);

            count = _fixture.Db.SetEntity<Role>().Count(p => p.UserId == 99);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task TestCountAsync()
        {
            var count = await _fixture.Db.SetEntity<User>().CountAsync();
            Assert.Equal(3, count);

            count = await _fixture.Db.SetEntity<Role>().CountAsync(p => p.UserId == 1);
            Assert.Equal(3, count);

            count = await _fixture.Db.SetEntity<Role>().CountAsync(p => p.UserId == 99);
            Assert.Equal(0, count);
        }

        #endregion

        #region Any

        [Fact]
        public void TestAny()
        {
            var count = _fixture.Db.SetEntity<User>().Any();
            Assert.Equal(true, count);

            count = _fixture.Db.SetEntity<Role>().Any(p => p.UserId == 99);
            Assert.Equal(false, count);
        }

        [Fact]
        public async Task TestAnyAsync()
        {
            var count = await _fixture.Db.SetEntity<User>().AnyAsync();
            Assert.Equal(true, count);

            count = await _fixture.Db.SetEntity<Role>().AnyAsync(p => p.UserId == 99);
            Assert.Equal(false, count);
        }

        #endregion

        #region Mulitple Mapping Test

        [Fact]
        public async Task TestFindMultipleMappingAsync()
        {
            var sql = @"SELECT * FROM Users
                        LEFT JOIN Cars ON Users.Id = Cars.UserId
                        LEFT JOIN Images On Users.Id = Images.UserId
                        WHERE Users.Deleted != 1";

            var users = (await _fixture.Db.SetEntity<User>().FindAllAsync<Car, Image>(sql)).ToList();
            Assert.Equal(3, users.Count);
            Assert.Equal(2, users[0].Cars.Count);
            Assert.Equal("Car1", users[0].Cars[0].CarName);
            Assert.Null(users[1].Cars);
            Assert.NotNull(users[0].Image);
            Assert.Equal("Image1", users[0].Image.Name);
            Assert.Null(users[1].Image);
            Assert.NotNull(users[2].Image);
            Assert.Equal("Image4", users[2].Image.Name);

            sql = @"SELECT * FROM Users
                    LEFT JOIN Cars ON Users.Id = Cars.UserId
                    LEFT JOIN CarOptions ON Cars.Id = CarOptions.CarId
                    LEFT JOIN Images On Users.Id = Images.UserId
                    WHERE Users.Deleted != 1";

            users = (await _fixture.Db.SetEntity<User>().FindAllAsync<Car, CarOption, Image>(sql)).ToList();
            Assert.Equal(3, users.Count);
            Assert.Equal(2, users[0].Cars.Count);
            Assert.Equal("Car1", users[0].Cars[0].CarName);
            Assert.Null(users[1].Cars);
            Assert.NotNull(users[0].Image);
            Assert.Equal("Image1", users[0].Image.Name);
            Assert.Null(users[1].Image);
            Assert.NotNull(users[2].Image);
            Assert.Equal("Image4", users[2].Image.Name);
        }

        #endregion
    }
}
