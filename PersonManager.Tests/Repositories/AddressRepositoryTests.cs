using PersonManager.Domain;
using PersonManager.Data;
using PersonManager.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PersonManager.Tests.Repositories
{
    public class AddressRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AddressRepoTestDb")
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_AddsAddress()
        {
            var db = GetDbContext();
            var repo = new AddressRepository(db);
            var address = new Address { Street = "Testowa 1" };
            var result = await repo.AddAsync(address, CancellationToken.None);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Testowa 1", result.Data.Street);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsAddress()
        {
            var db = GetDbContext();
            var repo = new AddressRepository(db);
            var address = new Address { Street = "FindMe 2" };
            await repo.AddAsync(address);
            await db.SaveChangesAsync();
            var result = await repo.GetByIdAsync(address.Id);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("FindMe 2", result.Data.Street);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAll()
        {
            var db = GetDbContext();
            var repo = new AddressRepository(db);
            await repo.AddAsync(new Address { Street = "A" });
            await db.SaveChangesAsync();
            await repo.AddAsync(new Address { Street = "B" });
            await db.SaveChangesAsync();
            var result = await repo.GetAllAsync();
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesAddress()
        {
            var db = GetDbContext();
            var repo = new AddressRepository(db);
            var address = new Address { Street = "Old" };
            await repo.AddAsync(address);
            await db.SaveChangesAsync();
            address.Street = "New";
            var result = await repo.UpdateAsync(address);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("New", result.Data.Street);
        }

        [Fact]
        public async Task DeleteAsync_DeletesAddress()
        {
            var db = GetDbContext();
            var repo = new AddressRepository(db);
            var address = new Address { Street = "Del" };
            await repo.AddAsync(address);
            var result = await repo.DeleteAsync(address.Id);
            Assert.True(result.Success);
        }

        [Fact]
        public void GetQueryable_ReturnsQueryable()
        {
            var db = GetDbContext();
            var repo = new AddressRepository(db);
            var queryable = repo.GetQueryable();
            Assert.NotNull(queryable);
            Assert.IsAssignableFrom<IQueryable<Address>>(queryable);
        }
    }
}
