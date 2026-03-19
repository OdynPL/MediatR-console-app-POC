using Moq;
using AutoMapper;
using PersonManager.Commands;
using PersonManager.Handlers;
using PersonManager.Services;
using PersonManager.DTO;
using PersonManager.Domain;

namespace PersonManager.Tests.Handlers
{
    public class CreatePersonCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccess()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var personServiceMock = new Mock<IPersonService>();
            var command = new CreatePersonCommand("Jan", 30);
            var person = new Person { Id = 1, Name = "Jan", Age = 30 };
            var resultDto = new PersonResponseDto { Id = 1, Name = "Jan", Age = 30, Success = true };

            mapperMock.Setup(m => m.Map<Person>(command)).Returns(person);
            mapperMock.Setup(m => m.Map<PersonResponseDto>(person)).Returns(resultDto);
            personServiceMock.Setup(s => s.AddPersonAsync(person, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Person> { Success = true, Data = person });

            var handler = new CreatePersonCommandHandler(mapperMock.Object, personServiceMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Jan", result.Name);
            Assert.Equal(30, result.Age);
        }

        [Fact]
        public async Task Handle_EmptyName_ReturnsError()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var personServiceMock = new Mock<IPersonService>();
            var command = new CreatePersonCommand("", 30);
            var handler = new CreatePersonCommandHandler(mapperMock.Object, personServiceMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Imię nie może być puste.", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_InvalidAge_ReturnsError()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var personServiceMock = new Mock<IPersonService>();
            var command = new CreatePersonCommand("Jan", 0);
            var handler = new CreatePersonCommandHandler(mapperMock.Object, personServiceMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Wiek musi być dodatni.", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ServiceThrowsException_ReturnsError()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var personServiceMock = new Mock<IPersonService>();
            var command = new CreatePersonCommand("Jan", 30);
            var person = new Person { Id = 1, Name = "Jan", Age = 30 };

            mapperMock.Setup(m => m.Map<Person>(command)).Returns(person);
            personServiceMock.Setup(s => s.AddPersonAsync(person, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Błąd bazy danych"));

            var handler = new CreatePersonCommandHandler(mapperMock.Object, personServiceMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Błąd bazy danych", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_MapsPersonCorrectly()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var personServiceMock = new Mock<IPersonService>();
            var command = new CreatePersonCommand("Anna", 25);
            var person = new Person { Id = 2, Name = "Anna", Age = 25 };
            var resultDto = new PersonResponseDto { Id = 2, Name = "Anna", Age = 25, Success = true };

            mapperMock.Setup(m => m.Map<Person>(command)).Returns(person);
            mapperMock.Setup(m => m.Map<PersonResponseDto>(person)).Returns(resultDto);
            personServiceMock.Setup(s => s.AddPersonAsync(person, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Person> { Success = true, Data = person });

            var handler = new CreatePersonCommandHandler(mapperMock.Object, personServiceMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Anna", result.Name);
            Assert.Equal(25, result.Age);
        }
    }
}
