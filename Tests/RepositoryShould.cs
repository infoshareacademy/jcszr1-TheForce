using BLL;
using BLL.Data;
using BLL.Data.Repositories;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace Tests
{

    public class TestWithSqlite : IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;
        public DrinkRepository Repository { get; private set; }

        public TestWithSqlite()
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<DrinkAppContext>()
                .UseSqlite(_connection)
                .Options;
            var dbContext = new DrinkAppContext(options);
            dbContext.Database.EnsureCreated();
            var data = new DrinkLoader().InitializeDrinksFromFile();
            // Add drinks to the database
            dbContext.AddRange(data);
            dbContext.SaveChanges();

            Repository = new DrinkRepository(dbContext);
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }

    public class RepositoryShould : IClassFixture<TestWithSqlite>
    {
        private readonly TestWithSqlite _fixture;

        public RepositoryShould(TestWithSqlite fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Get_all_drinks_from_the_database()
        {
            var drinks = _fixture.Repository.GetAllDrinks().ToList();
            drinks.Should().HaveCount(42);
        }

        [Fact]
        public void Not_get_empty_or_null_list_from_the_database()
        {
            var drinks = _fixture.Repository.GetAllDrinks().ToList();
            drinks.Should().NotBeNullOrEmpty();
        }
    }
}
