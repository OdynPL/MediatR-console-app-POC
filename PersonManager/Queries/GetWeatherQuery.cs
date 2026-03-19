using MediatR;
using PersonManager.DTO;

namespace PersonManager.Queries
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
