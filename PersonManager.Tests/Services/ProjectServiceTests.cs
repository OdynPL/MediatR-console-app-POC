using Moq;
using PersonManager.Domain;
using PersonManager.DTO;
using PersonManager.Services;
using PersonManager.UnitOfWork;

namespace PersonManager.Tests.Services
{
    public class ProjectServiceTests
    {
        [Fact]
        public async Task AddProjectAsync_AddsProjectAndReturnsSuccess()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IProjectRepository>();
            var project = new Project { Title = "TestProject", Members = new List<Person>() };
            repoMock.Setup(r => r.AddAsync(project, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Project> { Success = true, Data = project });
            unitOfWorkMock.Setup(u => u.ProjectRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var loggerMock = new Mock<ILoggerService>();
            var service = new ProjectService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.AddProjectAsync(project, CancellationToken.None);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("TestProject", result.Data.Title);
        }

        [Fact]
        public async Task GetProjectByIdAsync_ReturnsProject()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IProjectRepository>();
            var project = new Project { Title = "FindMe", Members = new List<Person>() };
            repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Project> { Success = true, Data = project });
            unitOfWorkMock.Setup(u => u.ProjectRepository).Returns(repoMock.Object);
            var loggerMock = new Mock<ILoggerService>();
            var service = new ProjectService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.GetProjectByIdAsync(1);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("FindMe", result.Data.Title);
        }

        [Fact]
        public async Task GetAllProjectsAsync_ReturnsAll()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IProjectRepository>();
            var projects = new List<Project> { new Project { Title = "A", Members = new List<Person>() }, new Project { Title = "B", Members = new List<Person>() } };
            repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<List<Project>> { Success = true, Data = projects });
            unitOfWorkMock.Setup(u => u.ProjectRepository).Returns(repoMock.Object);
            var loggerMock = new Mock<ILoggerService>();
            var service = new ProjectService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.GetAllProjectsAsync();
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task UpdateProjectAsync_UpdatesProject()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IProjectRepository>();
            var project = new Project { Title = "Old", Members = new List<Person>() };
            repoMock.Setup(r => r.UpdateAsync(project, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Project> { Success = true, Data = project });
            unitOfWorkMock.Setup(u => u.ProjectRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var loggerMock = new Mock<ILoggerService>();
            var service = new ProjectService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.UpdateProjectAsync(project);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Old", result.Data.Title);
        }

        [Fact]
        public async Task DeleteProjectAsync_DeletesProject()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IProjectRepository>();
            repoMock.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<bool> { Success = true, Data = true });
            unitOfWorkMock.Setup(u => u.ProjectRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var loggerMock = new Mock<ILoggerService>();
            var service = new ProjectService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.DeleteProjectAsync(1);
            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        [Fact]
        public void GetQueryableProjects_ReturnsQueryable()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IProjectRepository>();
            repoMock.Setup(r => r.GetQueryable()).Returns(new List<Project>().AsQueryable());
            unitOfWorkMock.Setup(u => u.ProjectRepository).Returns(repoMock.Object);
            var loggerMock = new Mock<ILoggerService>();
            var service = new ProjectService(unitOfWorkMock.Object, loggerMock.Object);
            var queryable = service.GetQueryableProjects();
            Assert.NotNull(queryable);
            Assert.IsAssignableFrom<IQueryable<Project>>(queryable);
        }
    }
}
