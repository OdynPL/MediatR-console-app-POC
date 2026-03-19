namespace PersonManager.DTO
{
    public class WeatherResponseDto
    {
        public string? City { get; set; }
        public string? Description { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
