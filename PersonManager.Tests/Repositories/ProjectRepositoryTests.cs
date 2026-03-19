using PersonManager.Domain;
using PersonManager.Data;
using Microsoft.EntityFrameworkCore;
namespace PersonManager.Tests.Repositories
{
    public class ProjectRepositoryTests
    {

        private readonly AppDbContext _db;
        private readonly ProjectRepository _repo;

        public ProjectRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"ProjectRepoTestDb_{Guid.NewGuid()}")
                .Options;
            _db = new AppDbContext(options);
            _repo = new ProjectRepository(_db);
        }

        [Fact]
        public async Task AddAsync_AddsProjectToDb()
        {
            var project = new Project { Title = "Test Project", Members = new List<Person>() };
            var result = await _repo.AddAsync(project, CancellationToken.None);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Test Project", result.Data.Title);
        }
        [Fact]
        public async Task GetByIdAsync_ReturnsProject()
        {
            var project = new Project { Title = "FindMe", Members = new List<Person>() };
            await _repo.AddAsync(project);
            await _db.SaveChangesAsync();
            var result = await _repo.GetByIdAsync(project.Id);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("FindMe", result.Data.Title);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAll()
        {
            await _repo.AddAsync(new Project { Title = "A", Members = new List<Person>() });
            await _db.SaveChangesAsync();
            await _repo.AddAsync(new Project { Title = "B", Members = new List<Person>() });
            await _db.SaveChangesAsync();
            var result = await _repo.GetAllAsync();
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProject()
        {
            var project = new Project { Title = "Old", Members = new List<Person>() };
            await _repo.AddAsync(project);
            await _db.SaveChangesAsync();
            project.Title = "New";
            var result = await _repo.UpdateAsync(project);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("New", result.Data.Title);
        }

        [Fact]
        public async Task DeleteAsync_DeletesProject()
        {
            var project = new Project { Title = "Del", Members = new List<Person>() };
            await _repo.AddAsync(project);
            var result = await _repo.DeleteAsync(project.Id);
            Assert.True(result.Success);
        }

        [Fact]
        public void GetQueryable_ReturnsQueryable()
        {
            var queryable = _repo.GetQueryable();
            Assert.NotNull(queryable);
            Assert.IsAssignableFrom<System.Linq.IQueryable<Project>>(queryable);
        }
    }
}
