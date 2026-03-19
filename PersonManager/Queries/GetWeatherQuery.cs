using MediatR;
using MediatRApp.DTO;

namespace MediatRApp.Queries
{
    public class GetWeatherQuery : IRequest<WeatherResponseDto>
    {
        public string City { get; }
        public GetWeatherQuery(string city)
        {
            City = city;
        }
    }
}
