using Moq;
using PersonManager.Domain;
using PersonManager.DTO;
using PersonManager.Repositories;
using PersonManager.Services;
using PersonManager.UnitOfWork;

namespace PersonManager.Tests.Services
{
    public class CompanyServiceTests
    {
        [Fact]
        public async Task AddCompanyAsync_AddsCompanyAndReturnsSuccess()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<ICompanyRepository>();
            var company = new Company { Name = "TestCo", Employees = new List<Person>() };
            repoMock.Setup(r => r.AddAsync(company, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Company> { Success = true, Data = company });
            unitOfWorkMock.Setup(u => u.CompanyRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var loggerMock = new Mock<ILoggerService>();
            var service = new CompanyService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.AddCompanyAsync(company, CancellationToken.None);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("TestCo", result.Data.Name);
        }

        [Fact]
        public async Task GetCompanyByIdAsync_ReturnsCompany()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<ICompanyRepository>();
            var company = new Company { Name = "FindMe", Employees = new List<Person>() };
            repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Company> { Success = true, Data = company });
            unitOfWorkMock.Setup(u => u.CompanyRepository).Returns(repoMock.Object);
            var loggerMock = new Mock<ILoggerService>();
            var service = new CompanyService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.GetCompanyByIdAsync(1);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("FindMe", result.Data.Name);
        }

        [Fact]
        public async Task GetAllCompaniesAsync_ReturnsAll()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<ICompanyRepository>();
            var companies = new List<Company> { new Company { Name = "A", Employees = new List<Person>() }, new Company { Name = "B", Employees = new List<Person>() } };
            repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<List<Company>> { Success = true, Data = companies });
            unitOfWorkMock.Setup(u => u.CompanyRepository).Returns(repoMock.Object);
            var loggerMock = new Mock<ILoggerService>();
            var service = new CompanyService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.GetAllCompaniesAsync();
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task UpdateCompanyAsync_UpdatesCompany()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<ICompanyRepository>();
            var company = new Company { Name = "Old", Employees = new List<Person>() };
            repoMock.Setup(r => r.UpdateAsync(company, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Company> { Success = true, Data = company });
            unitOfWorkMock.Setup(u => u.CompanyRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var loggerMock = new Mock<ILoggerService>();
            var service = new CompanyService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.UpdateCompanyAsync(company);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Old", result.Data.Name);
        }

        [Fact]
        public async Task DeleteCompanyAsync_DeletesCompany()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<ICompanyRepository>();
            repoMock.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<bool> { Success = true, Data = true });
            unitOfWorkMock.Setup(u => u.CompanyRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var loggerMock = new Mock<ILoggerService>();
            var service = new CompanyService(unitOfWorkMock.Object, loggerMock.Object);
            var result = await service.DeleteCompanyAsync(1);
            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        [Fact]
        public void GetQueryableCompanies_ReturnsQueryable()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<ICompanyRepository>();
            repoMock.Setup(r => r.GetQueryable()).Returns(new List<Company>().AsQueryable());
            unitOfWorkMock.Setup(u => u.CompanyRepository).Returns(repoMock.Object);
            var loggerMock = new Mock<ILoggerService>();
            var service = new CompanyService(unitOfWorkMock.Object, loggerMock.Object);
            var queryable = service.GetQueryableCompanies();
            Assert.NotNull(queryable);
            Assert.IsAssignableFrom<IQueryable<Company>>(queryable);
        }

        [Fact]
        public async Task CreateCompanyAsync_CreatesAndReturnsId()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<ICompanyRepository>();
            var company = new Company { Id = 42, Name = "Nowa", Employees = new List<Person>() };
            repoMock.Setup(r => r.AddAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RepositoryResult<Company> { Success = true, Data = company });
            unitOfWorkMock.Setup(u => u.CompanyRepository).Returns(repoMock.Object);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var loggerMock = new Mock<ILoggerService>();
            var service = new CompanyService(unitOfWorkMock.Object, loggerMock.Object);
            var id = await service.CreateCompanyAsync("Nowa");
            Assert.Equal(42, id);
        }
    }
}
