using PersonManager.Domain;
using PersonManager.Data;
using Microsoft.EntityFrameworkCore;

namespace PersonManager.Tests.Repositories
{
    public class CompanyRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"CompanyRepoTestDb_{Guid.NewGuid()}")
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_AddsCompany()
        {
            var db = GetDbContext();
            var repo = new CompanyRepository(db);
            var company = new Company { Name = "TestCo" };
            var result = await repo.AddAsync(company, CancellationToken.None);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("TestCo", result.Data.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCompany()
        {
            var db = GetDbContext();
            var repo = new CompanyRepository(db);
            var company = new Company { Name = "FindMe" };
            await repo.AddAsync(company);
            await db.SaveChangesAsync();
            var result = await repo.GetByIdAsync(company.Id);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("FindMe", result.Data.Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAll()
        {
            var db = GetDbContext();
            var repo = new CompanyRepository(db);
            await repo.AddAsync(new Company { Name = "A" });
            await db.SaveChangesAsync();
            await repo.AddAsync(new Company { Name = "B" });
            await db.SaveChangesAsync();
            var result = await repo.GetAllAsync();
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCompany()
        {
            var db = GetDbContext();
            var repo = new CompanyRepository(db);
            var company = new Company { Name = "Old" };
            await repo.AddAsync(company);
            await db.SaveChangesAsync();
            company.Name = "New";
            var result = await repo.UpdateAsync(company);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("New", result.Data.Name);
        }

        [Fact]
        public async Task DeleteAsync_DeletesCompany()
        {
            var db = GetDbContext();
            var repo = new CompanyRepository(db);
            var company = new Company { Name = "Del" };
            await repo.AddAsync(company);
            var result = await repo.DeleteAsync(company.Id);
            Assert.True(result.Success);
        }

        [Fact]
        public void GetQueryable_ReturnsQueryable()
        {
            var db = GetDbContext();
            var repo = new CompanyRepository(db);
            var queryable = repo.GetQueryable();
            Assert.NotNull(queryable);
            Assert.IsAssignableFrom<IQueryable<Company>>(queryable);
        }
    }
}
