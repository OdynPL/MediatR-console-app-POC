using Xunit;
using Moq;
using PersonManager.Handlers;
using PersonManager.Queries;
using PersonManager.Repositories;
using PersonManager.DTO;
using AutoMapper;

namespace PersonManager.Tests.Handlers
{
    public class GetPersonByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingPerson_ReturnsDto()
        {
            var repoMock = new Mock<IPersonRepository>();
            var mapperMock = new Mock<IMapper>();
            var handler = new GetPersonByIdQueryHandler(repoMock.Object, mapperMock.Object);
            var person = new Domain.Person { Id = 1, Name = "Jan", Age = 30 };
            var dto = new PersonResponseDto { Id = 1, Name = "Jan", Age = 30, Success = true };
            repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Domain.Person> { Success = true, Data = person });
            mapperMock.Setup(m => m.Map<PersonResponseDto>(person)).Returns(dto);
            var result = await handler.Handle(new GetPersonByIdQuery(1), CancellationToken.None);
            Assert.True(result.Success);
            Assert.Equal("Jan", result.Name);
        }

        [Fact]
        public async Task Handle_NotFound_ReturnsError()
        {
            var repoMock = new Mock<IPersonRepository>();
            var mapperMock = new Mock<IMapper>();
            var handler = new GetPersonByIdQueryHandler(repoMock.Object, mapperMock.Object);
            repoMock.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Domain.Person> { Success = false, ErrorMessage = "Not found" });
            var result = await handler.Handle(new GetPersonByIdQuery(99), CancellationToken.None);
            Assert.False(result.Success);
            Assert.Equal("Not found", result.ErrorMessage);
        }
    }
}
