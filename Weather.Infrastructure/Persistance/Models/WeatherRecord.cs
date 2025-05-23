namespace Weather.Infrastructure.Persistance.Models
{
    public class WeatherRecord
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string JsonPayload { get; set; } = null!;
    }
}
