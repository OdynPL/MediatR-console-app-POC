using System.Net;
using System.Net.Http.Json;
using Moq;
using Moq.Protected;
using PersonManager.Services;

namespace PersonManager.Tests.Services
{
    public class RestApiServiceTests
    {
        private RestApiService<TestDto> CreateService(HttpResponseMessage response)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var client = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
            return new RestApiService<TestDto>(factoryMock.Object, "TestApi", "api/test");
        }

        [Fact]
        public async Task GetAsync_ReturnsOk_WhenSuccess()
        {
            var dto = new TestDto { Id = 1, Name = "Test" };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(dto)
            };
            var service = CreateService(response);
            var result = await service.GetAsync(1);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dto.Id, result.Data.Id);
        }

        [Fact]
        public async Task GetAsync_ReturnsFail_WhenNotFound()
        {
            var response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("")
            };
            var service = CreateService(response);
            var result = await service.GetAsync(1);
            Assert.False(result.Success);
            Assert.Contains("Błąd pobierania", result.ErrorMessage);
        }

        [Fact]
        public async Task AddAsync_ReturnsOk_WhenSuccess()
        {
            var dto = new TestDto { Id = 2, Name = "Added" };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(dto)
            };
            var service = CreateService(response);
            var result = await service.AddAsync(dto);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dto.Name, result.Data.Name);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_WhenNoId()
        {
            var service = CreateService(new HttpResponseMessage(HttpStatusCode.OK));
            var dto = new TestDto { Name = "NoId" }; // No Id property set
            var result = await service.UpdateAsync(dto);
            Assert.False(result.Success);
            Assert.Contains("Wyjątek", result.ErrorMessage);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsOk_WhenSuccess()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };
            var service = CreateService(response);
            var result = await service.DeleteAsync(1);
            Assert.True(result.Success);
            Assert.True(result.Data);
        }
    }
}
