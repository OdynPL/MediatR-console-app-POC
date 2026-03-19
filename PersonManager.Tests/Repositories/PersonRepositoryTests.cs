using PersonManager.Domain;
using PersonManager.Data;
using PersonManager.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PersonManager.Tests.Repositories
{
    public class PersonRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"PersonRepoTestDb_{Guid.NewGuid()}")
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_AddsPerson()
        {
            var db = GetDbContext();
            var repo = new PersonRepository(db);
            var person = new Person { Name = "Jan", Age = 30 };
            var result = await repo.AddAsync(person, CancellationToken.None);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Jan", result.Data.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsPerson()
        {
            var db = GetDbContext();
            var repo = new PersonRepository(db);
            var person = new Person { Name = "Anna", Age = 25 };
            await repo.AddAsync(person);
            await db.SaveChangesAsync();
            var result = await repo.GetByIdAsync(person.Id);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Anna", result.Data.Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAll()
        {
            var db = GetDbContext();
            var repo = new PersonRepository(db);
            await repo.AddAsync(new Person { Name = "A", Age = 1 });
            await db.SaveChangesAsync();
            await repo.AddAsync(new Person { Name = "B", Age = 2 });
            await db.SaveChangesAsync();
            var result = await repo.GetAllAsync();
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesPerson()
        {
            var db = GetDbContext();
            var repo = new PersonRepository(db);
            var person = new Person { Name = "Old", Age = 10 };
            await repo.AddAsync(person);
            await db.SaveChangesAsync();
            person.Name = "New";
            var result = await repo.UpdateAsync(person);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("New", result.Data.Name);
        }

        [Fact]
        public async Task DeleteAsync_DeletesPerson()
        {
            var db = GetDbContext();
            var repo = new PersonRepository(db);
            var person = new Person { Name = "Del", Age = 20 };
            await repo.AddAsync(person);
            var result = await repo.DeleteAsync(person.Id);
            Assert.True(result.Success);
        }

        [Fact]
        public void GetQueryable_ReturnsQueryable()
        {
            var db = GetDbContext();
            var repo = new PersonRepository(db);
            var queryable = repo.GetQueryable();
            Assert.NotNull(queryable);
            Assert.IsAssignableFrom<IQueryable<Person>>(queryable);
        }
    }
}
