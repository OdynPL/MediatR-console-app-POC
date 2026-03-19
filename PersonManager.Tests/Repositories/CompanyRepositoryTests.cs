using PersonManager.Domain;
using PersonManager.Data;
using Microsoft.EntityFrameworkCore;
using PersonManager.Repositories;
using PersonManager.Specifications;
using System.Linq.Expressions;

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
            var company = new Company { Name = "TestCo", Employees = new List<Person>() };
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
            var company = new Company { Name = "FindMe", Employees = new List<Person>() };
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
            await repo.AddAsync(new Company { Name = "A", Employees = new List<Person>() });
            await db.SaveChangesAsync();
            await repo.AddAsync(new Company { Name = "B", Employees = new List<Person>() });
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
            var company = new Company { Name = "Old", Employees = new List<Person>() };
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
            var company = new Company { Name = "Del", Employees = new List<Person>() };
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

        [Fact]
        public async Task GetBySpecificationAsync_ReturnsFilteredCompanies_ByName()
        {
            var db = GetDbContext();
            var repo = new CompanyRepository(db);
            var companies = new List<Company>
            {
                new Company { Name = "A", Employees = new List<Person>() },
                new Company { Name = "B", Employees = new List<Person>() },
                new Company { Name = "C", Employees = new List<Person>() }
            };
            foreach (var c in companies)
                await repo.AddAsync(c);
            await db.SaveChangesAsync();

            // Specyfikacja po nazwie firmy (możesz ją przenieść do osobnego pliku)
            var spec = new SpecificationByName("B");
            var result = await repo.GetBySpecificationAsync(spec);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data);
            Assert.Equal("B", result.Data[0].Name);
        }
    }

    public class SpecificationByName : ISpecification<Company>
    {
        public Expression<Func<Company, bool>> Criteria { get; }
        public SpecificationByName(string name)
        {
            Criteria = c => c.Name == name;
        }
    }
}
