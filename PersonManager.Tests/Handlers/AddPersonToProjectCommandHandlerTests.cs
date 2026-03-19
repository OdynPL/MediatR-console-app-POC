using PersonManager.Handlers;
using PersonManager.Services;
using PersonManager.Domain;
using PersonManager.DTO;
using Moq;
using PersonManager.Commands;

namespace PersonManager.Tests.Handlers
{
    public class AddPersonToProjectCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_AddsPersonToProject()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            var personServiceMock = new Mock<IPersonService>();
            var handler = new AddPersonToProjectCommandHandler(projectServiceMock.Object, personServiceMock.Object);
            var person = new Person { Id = 1, Name = "Jan" };
            var project = new Project { Id = 2, Title = "Test", Members = new System.Collections.Generic.List<Person>() };
            personServiceMock.Setup(s => s.GetPersonByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Person> { Success = true, Data = person });
            projectServiceMock.Setup(s => s.GetProjectByIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Project> { Success = true, Data = project });
            projectServiceMock.Setup(s => s.UpdateProjectAsync(project, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Project> { Success = true, Data = project });
            var command = new AddPersonToProjectCommand { PersonId = 1, ProjectId = 2 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Contains(person, project.Members);
        }
    }
}
