using Moq;
using PersonManager.Handlers;
using PersonManager.Queries;
using PersonManager.Services;

namespace PersonManager.Tests.Handlers
{
    public class GetWeatherQueryHandlerTests
    {
        [Fact]
        public async Task Handle_EmptyCity_ReturnsError()
        {
            var weatherServiceMock = new Mock<IWeatherService>();
            var handler = new GetWeatherQueryHandler(weatherServiceMock.Object);
            var result = await handler.Handle(new GetWeatherQuery(""), CancellationToken.None);
            Assert.False(result.Success);
            Assert.Equal("Miasto nie może być puste.", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ValidCity_ReturnsWeather()
        {
            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(s => s.GetWeather("Warszawa")).Returns("Słonecznie");
            var handler = new GetWeatherQueryHandler(weatherServiceMock.Object);
            var result = await handler.Handle(new GetWeatherQuery("Warszawa"), CancellationToken.None);
            Assert.True(result.Success);
            Assert.Equal("Warszawa", result.City);
            Assert.Equal("Słonecznie", result.Description);
        }

        [Fact]
        public async Task Handle_ServiceThrowsException_ReturnsError()
        {
            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(s => s.GetWeather(It.IsAny<string>())).Throws(new Exception("Błąd API pogody"));
            var handler = new GetWeatherQueryHandler(weatherServiceMock.Object);
            var result = await handler.Handle(new GetWeatherQuery("Kraków"), CancellationToken.None);
            Assert.False(result.Success);
            Assert.Equal("Błąd API pogody", result.ErrorMessage);
        }
    }
}
