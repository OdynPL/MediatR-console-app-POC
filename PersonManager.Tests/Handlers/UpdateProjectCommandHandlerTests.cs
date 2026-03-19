using Xunit;
using Moq;
using PersonManager.Handlers;
using PersonManager.Services;
using PersonManager.Domain;
using PersonManager.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PersonManager.Tests.Handlers
{
    public class UpdateProjectCommandHandlerTests
    {
        [Fact]
        public async Task Handle_UpdatesProjectMembersAndTitle()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            var personServiceMock = new Mock<IPersonService>();
            var project = new Project { Id = 1, Title = "OldTitle", Members = new List<Person> { new Person { Id = 2, Name = "Jan" } } };
            var memberIds = new List<int> { 3, 4 };
            var person3 = new Person { Id = 3, Name = "Anna" };
            var person4 = new Person { Id = 4, Name = "Piotr" };
            projectServiceMock.Setup(s => s.GetProjectByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PersonManager.DTO.RepositoryResult<Project> { Success = true, Data = project });
            projectServiceMock.Setup(s => s.UpdateProjectAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PersonManager.DTO.RepositoryResult<Project> { Success = true, Data = project });
            personServiceMock.Setup(s => s.GetPersonByIdAsync(3, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PersonManager.DTO.RepositoryResult<Person> { Success = true, Data = person3 });
            personServiceMock.Setup(s => s.GetPersonByIdAsync(4, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PersonManager.DTO.RepositoryResult<Person> { Success = true, Data = person4 });

            var handler = new UpdateProjectCommandHandler(projectServiceMock.Object, personServiceMock.Object);
            var command = new UpdateProjectCommand { Id = 1, Title = "NewTitle", MemberIds = memberIds };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal("NewTitle", project.Title);
            Assert.Equal(2, project.Members.Count);
            Assert.Contains(project.Members, m => m.Id == 3);
            Assert.Contains(project.Members, m => m.Id == 4);
        }
    }
}
