using Weather.Application.Dtos;

namespace Weather.Application.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherDto?> GetLiveOrCachedAsync(CancellationToken cancellation);
    }
}
