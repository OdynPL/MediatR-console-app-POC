using PersonManager.Domain;
using PersonManager.Data;
using Microsoft.EntityFrameworkCore;
namespace PersonManager.Tests.Repositories
{
    public class ProjectRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_AddsProjectToDb()
        {
            // Arrange
            var db = GetDbContext();
            var repo = new ProjectRepository(db);
            var project = new Project { Title = "Test Project", Members = new List<Person>() };

            // Act
            var result = await repo.AddAsync(project, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Test Project", result.Data.Title);
        }
        [Fact]
        public async Task GetByIdAsync_ReturnsProject()
        {
            var db = GetDbContext();
            var repo = new ProjectRepository(db);
            var project = new Project { Title = "FindMe", Members = new List<Person>() };
            await repo.AddAsync(project);
            await db.SaveChangesAsync();
            var result = await repo.GetByIdAsync(project.Id);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("FindMe", result.Data.Title);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAll()
        {
            var db = GetDbContext();
            var repo = new ProjectRepository(db);
            await repo.AddAsync(new Project { Title = "A", Members = new List<Person>() });
            await db.SaveChangesAsync();
            await repo.AddAsync(new Project { Title = "B", Members = new List<Person>() });
            await db.SaveChangesAsync();
            var result = await repo.GetAllAsync();
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProject()
        {
            var db = GetDbContext();
            var repo = new ProjectRepository(db);
            var project = new Project { Title = "Old", Members = new List<Person>() };
            await repo.AddAsync(project);
            await db.SaveChangesAsync();
            project.Title = "New";
            var result = await repo.UpdateAsync(project);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("New", result.Data.Title);
        }

        [Fact]
        public async Task DeleteAsync_DeletesProject()
        {
            var db = GetDbContext();
            var repo = new ProjectRepository(db);
            var project = new Project { Title = "Del", Members = new List<Person>() };
            await repo.AddAsync(project);
            var result = await repo.DeleteAsync(project.Id);
            Assert.True(result.Success);
        }

        [Fact]
        public void GetQueryable_ReturnsQueryable()
        {
            var db = GetDbContext();
            var repo = new ProjectRepository(db);
            var queryable = repo.GetQueryable();
            Assert.NotNull(queryable);
            Assert.IsAssignableFrom<System.Linq.IQueryable<Project>>(queryable);
        }
    }
}
