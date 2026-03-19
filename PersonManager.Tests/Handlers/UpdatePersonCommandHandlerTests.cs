using PersonManager.Handlers;
using PersonManager.Commands;
using PersonManager.Repositories;
using PersonManager.Domain;
using AutoMapper;
using PersonManager.DTO;
using Moq;

namespace PersonManager.Tests.Handlers
{
    public class UpdatePersonCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingPerson_UpdatesAndReturnsDto()
        {
            var repoMock = new Mock<IPersonRepository>();
            var mapperMock = new Mock<IMapper>();
            var handler = new UpdatePersonCommandHandler(repoMock.Object, mapperMock.Object);
            var command = new UpdatePersonCommand { Id = 1, Name = "Nowe Imię", Age = 40 };
            var person = new Person { Id = 1, Name = "Stare Imię", Age = 30 };
            // Mock GetByIdAsync zwraca istniejącą osobę
            repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<Person>.Ok(person));
            // Mock UpdateAsync zwraca sukces
            repoMock.Setup(r => r.UpdateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<Person>.Ok(person));
            // Mapper mapuje request na person (symulacja)
            mapperMock.Setup(m => m.Map(command, person)).Callback(() => { person.Name = command.Name; person.Age = command.Age; });
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.True(result.Success);
            Assert.Equal("Nowe Imię", result.Name);
            Assert.Equal(40, result.Age);
        }

        [Fact]
        public async Task Handle_UpdateFails_ReturnsError()
        {
            var repoMock = new Mock<IPersonRepository>();
            var mapperMock = new Mock<IMapper>();
            var handler = new UpdatePersonCommandHandler(repoMock.Object, mapperMock.Object);
            var command = new UpdatePersonCommand { Id = 1, Name = "Nowe Imię", Age = 40 };
            var person = new Person { Id = 1, Name = "Stare Imię", Age = 30 };
            // Mock GetByIdAsync zwraca istniejącą osobę
            repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<Person>.Ok(person));
            // Mock UpdateAsync zwraca błąd
            repoMock.Setup(r => r.UpdateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<Person>.Fail("Błąd"));
            mapperMock.Setup(m => m.Map(command, person)).Callback(() => { person.Name = command.Name; person.Age = command.Age; });
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.False(result.Success);
            Assert.Equal("Błąd", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_PersonNotFound_ReturnsError()
        {
            var repoMock = new Mock<IPersonRepository>();
            var mapperMock = new Mock<IMapper>();
            var handler = new UpdatePersonCommandHandler(repoMock.Object, mapperMock.Object);
            var command = new UpdatePersonCommand { Id = 99, Name = "Nieistniejący", Age = 10 };
            // Mock GetByIdAsync zwraca błąd
            repoMock.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<Person>.Fail("not found"));
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.False(result.Success);
            Assert.Equal("not found", result.ErrorMessage);
        }
    }
}
