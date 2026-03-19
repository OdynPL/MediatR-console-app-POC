using Moq;
using PersonManager.Domain;
using PersonManager.DTO;
using PersonManager.Repositories;
using PersonManager.Services;
using PersonManager.UnitOfWork;

namespace PersonManager.Tests.Services
{
    public class PersonServiceTests
    {
        [Fact]
        public async Task AddPersonAsync_AddsPersonAndReturnsSuccess()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IPersonRepository>();
            var person = new Person { Name = "Jan", Age = 30 };
            repoMock.Setup(r => r.AddAsync(person, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Person> { Success = true, Data = person });
            unitOfWorkMock.Setup(u => u.PersonRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var service = new PersonService(unitOfWorkMock.Object);

            var result = await service.AddPersonAsync(person, CancellationToken.None);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Jan", result.Data.Name);
        }

        [Fact]
        public async Task AddPersonAsync_WhenRepositoryFails_ReturnsFailure()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IPersonRepository>();
            var person = new Person { Name = "Jan", Age = 30 };
            repoMock.Setup(r => r.AddAsync(person, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<Person>.Fail("error"));
            unitOfWorkMock.Setup(u => u.PersonRepository).Returns(repoMock.Object);
            var service = new PersonService(unitOfWorkMock.Object);

            var result = await service.AddPersonAsync(person, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("error", result.ErrorMessage);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ReturnsPerson_WhenFound()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IPersonRepository>();
            var person = new Person { Id = 1, Name = "Jan", Age = 30 };
            repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<Person>.Ok(person));
            unitOfWorkMock.Setup(u => u.PersonRepository).Returns(repoMock.Object);
            var service = new PersonService(unitOfWorkMock.Object);

            var result = await service.GetPersonByIdAsync(1, CancellationToken.None);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ReturnsFailure_WhenNotFound()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IPersonRepository>();
            repoMock.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<Person>.Fail("not found"));
            unitOfWorkMock.Setup(u => u.PersonRepository).Returns(repoMock.Object);
            var service = new PersonService(unitOfWorkMock.Object);

            var result = await service.GetPersonByIdAsync(2, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("not found", result.ErrorMessage);
        }

        [Fact]
        public async Task GetAllPersonsAsync_ReturnsListOfPersons()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IPersonRepository>();
            var people = new List<Person> { new Person { Name = "Jan", Age = 30 }, new Person { Name = "Anna", Age = 25 } };
            repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<List<Person>>.Ok(people));
            unitOfWorkMock.Setup(u => u.PersonRepository).Returns(repoMock.Object);
            var service = new PersonService(unitOfWorkMock.Object);

            var result = await service.GetAllPersonsAsync(CancellationToken.None);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task GetAllPersonsAsync_ReturnsFailure_WhenRepositoryFails()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IPersonRepository>();
            repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<List<Person>>.Fail("db error"));
            unitOfWorkMock.Setup(u => u.PersonRepository).Returns(repoMock.Object);
            var service = new PersonService(unitOfWorkMock.Object);

            var result = await service.GetAllPersonsAsync(CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("db error", result.ErrorMessage);
        }

        [Fact]
        public async Task UpdatePersonAsync_UpdatesPersonAndReturnsSuccess()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IPersonRepository>();
            var person = new Person { Id = 1, Name = "Jan", Age = 30 };
            repoMock.Setup(r => r.UpdateAsync(person, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<Person>.Ok(person));
            unitOfWorkMock.Setup(u => u.PersonRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var service = new PersonService(unitOfWorkMock.Object);

            var result = await service.UpdatePersonAsync(person, CancellationToken.None);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
        }

        [Fact]
        public async Task UpdatePersonAsync_ReturnsFailure_WhenRepositoryFails()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IPersonRepository>();
            var person = new Person { Id = 1, Name = "Jan", Age = 30 };
            repoMock.Setup(r => r.UpdateAsync(person, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<Person>.Fail("update error"));
            unitOfWorkMock.Setup(u => u.PersonRepository).Returns(repoMock.Object);
            var service = new PersonService(unitOfWorkMock.Object);

            var result = await service.UpdatePersonAsync(person, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("update error", result.ErrorMessage);
        }

        [Fact]
        public async Task DeletePersonAsync_DeletesPersonAndReturnsSuccess()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IPersonRepository>();
            repoMock.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<bool>.Ok(true));
            unitOfWorkMock.Setup(u => u.PersonRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var service = new PersonService(unitOfWorkMock.Object);

            var result = await service.DeletePersonAsync(1, CancellationToken.None);

            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeletePersonAsync_ReturnsFailure_WhenRepositoryFails()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IPersonRepository>();
            repoMock.Setup(r => r.DeleteAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RepositoryResult<bool>.Fail("delete error"));
            unitOfWorkMock.Setup(u => u.PersonRepository).Returns(repoMock.Object);
            var service = new PersonService(unitOfWorkMock.Object);

            var result = await service.DeletePersonAsync(2, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("delete error", result.ErrorMessage);
        }

        [Fact]
        public void GetQueryablePersons_ReturnsQueryable()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IPersonRepository>();
            var people = new List<Person> { new Person { Name = "Jan", Age = 30 } }.AsQueryable();
            repoMock.Setup(r => r.GetQueryable()).Returns(people);
            unitOfWorkMock.Setup(u => u.PersonRepository).Returns(repoMock.Object);
            var service = new PersonService(unitOfWorkMock.Object);

            var result = service.GetQueryablePersons();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Jan", result.First().Name);
        }
    }
}
