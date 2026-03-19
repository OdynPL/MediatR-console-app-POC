using PersonManager.Domain;
using PersonManager.Data;
using PersonManager.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PersonManager.Tests.Repositories
{
    public class AddressRepositoryTests
    {

        private readonly AppDbContext _db;
        private readonly AddressRepository _repo;

        public AddressRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"AddressRepoTestDb_{Guid.NewGuid()}")
                .Options;
            _db = new AppDbContext(options);
            _repo = new AddressRepository(_db);
        }

        [Fact]
        public async Task AddAsync_AddsAddress()
        {
            var address = new Address { Street = "Testowa 1" };
            var result = await _repo.AddAsync(address, CancellationToken.None);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Testowa 1", result.Data.Street);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsAddress()
        {
            var address = new Address { Street = "FindMe 2" };
            await _repo.AddAsync(address);
            await _db.SaveChangesAsync();
            var result = await _repo.GetByIdAsync(address.Id);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("FindMe 2", result.Data.Street);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAll()
        {
            await _repo.AddAsync(new Address { Street = "A" });
            await _db.SaveChangesAsync();
            await _repo.AddAsync(new Address { Street = "B" });
            await _db.SaveChangesAsync();
            var result = await _repo.GetAllAsync();
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesAddress()
        {
            var address = new Address { Street = "Old" };
            await _repo.AddAsync(address);
            await _db.SaveChangesAsync();
            address.Street = "New";
            var result = await _repo.UpdateAsync(address);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("New", result.Data.Street);
        }

        [Fact]
        public async Task DeleteAsync_DeletesAddress()
        {
            var address = new Address { Street = "Del" };
            await _repo.AddAsync(address);
            var result = await _repo.DeleteAsync(address.Id);
            Assert.True(result.Success);
        }

        [Fact]
        public void GetQueryable_ReturnsQueryable()
        {
            var queryable = _repo.GetQueryable();
            Assert.NotNull(queryable);
            Assert.IsAssignableFrom<IQueryable<Address>>(queryable);
        }
    }
}
