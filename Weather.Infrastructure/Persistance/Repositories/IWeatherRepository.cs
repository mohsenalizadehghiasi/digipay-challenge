using Weather.Infrastructure.Persistance.Models;

namespace Weather.Infrastructure.Persistance.Repositories
{
    public interface IWeatherRepository
    {
        Task AddAsync(WeatherRecord record);
        Task<WeatherRecord?> GetLatestAsync();
    }
}
