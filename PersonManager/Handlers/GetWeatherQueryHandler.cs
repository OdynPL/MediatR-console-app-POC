using MediatR;
using MediatRApp.Queries;
using MediatRApp.Services;
using MediatRApp.DTO;
namespace MediatRApp.Handlers
{
    public class GetWeatherQueryHandler : IRequestHandler<GetWeatherQuery, WeatherResponseDto>
    {
        private readonly IWeatherService _weatherService;
        public GetWeatherQueryHandler(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        public async Task<WeatherResponseDto> Handle(GetWeatherQuery request, CancellationToken cancellationToken)
        {
            var response = new WeatherResponseDto { City = request.City };
            if (string.IsNullOrWhiteSpace(request.City))
            {
                response.Success = false;
                response.ErrorMessage = "Miasto nie może być puste.";
                return response;
            }
            try
            {
                await Task.Delay(10, cancellationToken);
                response.Description = _weatherService.GetWeather(request.City);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}
