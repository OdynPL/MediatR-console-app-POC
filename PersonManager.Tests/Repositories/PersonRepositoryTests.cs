using PersonManager.Domain;
using PersonManager.Data;
using PersonManager.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PersonManager.Tests.Repositories
{
    public class PersonRepositoryTests
    {

        private readonly AppDbContext _db;
        private readonly PersonRepository _repo;

        public PersonRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"PersonRepoTestDb_{Guid.NewGuid()}")
                .Options;
            _db = new AppDbContext(options);
            _repo = new PersonRepository(_db);
        }

        [Fact]
        public async Task AddAsync_AddsPerson()
        {
            var person = new Person { Name = "Jan", Age = 30 };
            var result = await _repo.AddAsync(person, CancellationToken.None);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Jan", result.Data.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsPerson()
        {
            var person = new Person { Name = "Anna", Age = 25 };
            await _repo.AddAsync(person);
            await _db.SaveChangesAsync();
            var result = await _repo.GetByIdAsync(person.Id);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Anna", result.Data.Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAll()
        {
            await _repo.AddAsync(new Person { Name = "A", Age = 1 });
            await _db.SaveChangesAsync();
            await _repo.AddAsync(new Person { Name = "B", Age = 2 });
            await _db.SaveChangesAsync();
            var result = await _repo.GetAllAsync();
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesPerson()
        {
            var person = new Person { Name = "Old", Age = 10 };
            await _repo.AddAsync(person);
            await _db.SaveChangesAsync();
            person.Name = "New";
            var result = await _repo.UpdateAsync(person);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("New", result.Data.Name);
        }

        [Fact]
        public async Task DeleteAsync_DeletesPerson()
        {
            var person = new Person { Name = "Del", Age = 20 };
            await _repo.AddAsync(person);
            var result = await _repo.DeleteAsync(person.Id);
            Assert.True(result.Success);
        }

        [Fact]
        public void GetQueryable_ReturnsQueryable()
        {
            var queryable = _repo.GetQueryable();
            Assert.NotNull(queryable);
            Assert.IsAssignableFrom<IQueryable<Person>>(queryable);
        }
    }
}
