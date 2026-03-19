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

        private readonly AppDbContext _db;
        private readonly CompanyRepository _repo;

        public CompanyRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"CompanyRepoTestDb_{Guid.NewGuid()}")
                .Options;
            _db = new AppDbContext(options);
            _repo = new CompanyRepository(_db);
        }

        [Fact]
        public async Task AddAsync_AddsCompany()
        {
            var company = new Company { Name = "TestCo", Employees = new List<Person>() };
            var result = await _repo.AddAsync(company, CancellationToken.None);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("TestCo", result.Data.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCompany()
        {
            var company = new Company { Name = "FindMe", Employees = new List<Person>() };
            await _repo.AddAsync(company);
            await _db.SaveChangesAsync();
            var result = await _repo.GetByIdAsync(company.Id);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("FindMe", result.Data.Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAll()
        {
            await _repo.AddAsync(new Company { Name = "A", Employees = new List<Person>() });
            await _db.SaveChangesAsync();
            await _repo.AddAsync(new Company { Name = "B", Employees = new List<Person>() });
            await _db.SaveChangesAsync();
            var result = await _repo.GetAllAsync();
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCompany()
        {
            var company = new Company { Name = "Old", Employees = new List<Person>() };
            await _repo.AddAsync(company);
            await _db.SaveChangesAsync();
            company.Name = "New";
            var result = await _repo.UpdateAsync(company);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("New", result.Data.Name);
        }

        [Fact]
        public async Task DeleteAsync_DeletesCompany()
        {
            var company = new Company { Name = "Del", Employees = new List<Person>() };
            await _repo.AddAsync(company);
            var result = await _repo.DeleteAsync(company.Id);
            Assert.True(result.Success);
        }

        [Fact]
        public void GetQueryable_ReturnsQueryable()
        {
            var queryable = _repo.GetQueryable();
            Assert.NotNull(queryable);
            Assert.IsAssignableFrom<IQueryable<Company>>(queryable);
        }

        [Fact]
        public async Task GetBySpecificationAsync_ReturnsFilteredCompanies_ByName()
        {
            var companies = new List<Company>
            {
                new Company { Name = "A", Employees = new List<Person>() },
                new Company { Name = "B", Employees = new List<Person>() },
                new Company { Name = "C", Employees = new List<Person>() }
            };
            foreach (var c in companies)
                await _repo.AddAsync(c);
            await _db.SaveChangesAsync();

            var spec = new SpecificationByName("B");
            var result = await _repo.GetBySpecificationAsync(spec);
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
