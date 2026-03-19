using PersonManager.Handlers;
using PersonManager.Commands;
using PersonManager.Repositories;
using PersonManager.Domain;
using Moq;

namespace PersonManager.Tests.Handlers
{
    public class DeletePersonCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingPerson_DeletesAndReturnsTrue()
        {
            var repoMock = new Mock<IPersonRepository>();
            var handler = new DeletePersonCommandHandler(repoMock.Object);
            var command = new DeletePersonCommand { Id = 1 };
            repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DTO.RepositoryResult<Person> { Success = true, Data = new Person { Id = 1, Name = "Jan", Age = 30 } });
            repoMock.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DTO.RepositoryResult<bool> { Success = true, Data = true });
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_NonExistingPerson_ReturnsFalse()
        {
            var repoMock = new Mock<IPersonRepository>();
            var handler = new DeletePersonCommandHandler(repoMock.Object);
            var command = new DeletePersonCommand { Id = 99 };
            // Zwracamy result z Success = false, Data = null
            repoMock.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DTO.RepositoryResult<Person> { Success = false, Data = null });
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.False(result);
        }
    }
}
