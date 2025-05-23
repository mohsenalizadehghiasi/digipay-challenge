
namespace Weather.Application.Dtos
{
    public class WeatherDto
    {
        public DateTime Timestamp { get; set; }
        public string RawJson { get; set; } = null!;
    }
}
