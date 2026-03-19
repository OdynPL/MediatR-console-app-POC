namespace PersonManager.Services
{
    public interface IWeatherService
    {
        string GetWeather(string city);
    }

    public class WeatherService : IWeatherService
    {
        public string GetWeather(string city)
        {
            return $"Pogoda w {city}: Słonecznie, 20°C";
        }
    }
}
