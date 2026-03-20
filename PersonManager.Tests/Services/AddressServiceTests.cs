using Moq;
using PersonManager.Domain;
using PersonManager.DTO;
using PersonManager.Repositories;
using PersonManager.Services;
using PersonManager.UnitOfWork;

namespace PersonManager.Tests.Services
{
    public class AddressServiceTests
    {
        [Fact]
        public async Task AddAddressAsync_AddsAddressAndReturnsSuccess()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IAddressRepository>();
            var address = new Address { Street = "Testowa 1" };
            repoMock.Setup(r => r.AddAsync(address, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Address> { Success = true, Data = address });
            unitOfWorkMock.Setup(u => u.AddressRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var loggerMock = new Mock<ILoggerService>();
            var service = new AddressService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.AddAddressAsync(address, CancellationToken.None);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Testowa 1", result.Data.Street);
        }

        [Fact]
        public async Task GetAddressByIdAsync_ReturnsAddress()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IAddressRepository>();
            var address = new Address { Street = "FindMe 2" };
            repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Address> { Success = true, Data = address });
            unitOfWorkMock.Setup(u => u.AddressRepository).Returns(repoMock.Object);
            var loggerMock = new Mock<ILoggerService>();
            var service = new AddressService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.GetAddressByIdAsync(1);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("FindMe 2", result.Data.Street);
        }

        [Fact]
        public async Task GetAllAddressesAsync_ReturnsAll()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IAddressRepository>();
            var addresses = new List<Address> { new Address { Street = "A" }, new Address { Street = "B" } };
            repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<List<Address>> { Success = true, Data = addresses });
            unitOfWorkMock.Setup(u => u.AddressRepository).Returns(repoMock.Object);
            var loggerMock = new Mock<ILoggerService>();
            var service = new AddressService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.GetAllAddressesAsync();
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task UpdateAddressAsync_UpdatesAddress()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IAddressRepository>();
            var address = new Address { Street = "Old" };
            repoMock.Setup(r => r.UpdateAsync(address, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Address> { Success = true, Data = address });
            unitOfWorkMock.Setup(u => u.AddressRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var loggerMock = new Mock<ILoggerService>();
            var service = new AddressService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.UpdateAddressAsync(address);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Old", result.Data.Street);
        }

        [Fact]
        public async Task DeleteAddressAsync_DeletesAddress()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IAddressRepository>();
            repoMock.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<bool> { Success = true, Data = true });
            unitOfWorkMock.Setup(u => u.AddressRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var loggerMock = new Mock<ILoggerService>();
            var service = new AddressService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.DeleteAddressAsync(1);
            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        [Fact]
        public void GetQueryableAddresses_ReturnsQueryable()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IAddressRepository>();
            repoMock.Setup(r => r.GetQueryable()).Returns(new List<Address>().AsQueryable());
            unitOfWorkMock.Setup(u => u.AddressRepository).Returns(repoMock.Object);
            var loggerMock = new Mock<ILoggerService>();
            var service = new AddressService(unitOfWorkMock.Object, loggerMock.Object);
            var queryable = service.GetQueryableAddresses();
            Assert.NotNull(queryable);
            Assert.IsAssignableFrom<IQueryable<Address>>(queryable);
        }

        [Fact]
        public async Task CreateAddressAsync_CreatesAndReturnsId()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IAddressRepository>();
            var address = new Address { Id = 42, Street = "Nowa", City = "Miasto", Country = "PL" };
            repoMock.Setup(r => r.AddAsync(It.IsAny<Address>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Address> { Success = true, Data = address });
            unitOfWorkMock.Setup(u => u.AddressRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var loggerMock = new Mock<ILoggerService>();
            var service = new AddressService(unitOfWorkMock.Object, loggerMock.Object);
            var id = await service.CreateAddressAsync("Nowa", "Miasto", "PL");
            Assert.Equal(42, id);
        }
    }
}
