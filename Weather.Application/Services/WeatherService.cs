using Weather.Application.Interfaces;
using Weather.Application.Dtos;
using Weather.Infrastructure.Persistance.Models;
using Weather.Infrastructure.Persistance.Repositories;
using Microsoft.Extensions.Logging;
using Weather.Infrastructure.Providers;

namespace Weather.Application.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherProvider _provider;
        private readonly IWeatherRepository _repository;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(
           IWeatherProvider provider,
           IWeatherRepository repository,
           ILogger<WeatherService> logger)
        {
            _provider = provider;
            _repository = repository;
            _logger = logger;
        }


        public async Task<WeatherDto?> GetLiveOrCachedAsync(CancellationToken cancellation)
        {
            string? rawJson;
            try
            {
                rawJson = await _provider.GetRawForecastJsonAsync(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Provider call failed.");
                rawJson = null;
            }

            if (!string.IsNullOrEmpty(rawJson))
            {
                var record = new WeatherRecord
                {
                    Timestamp = DateTime.UtcNow,
                    JsonPayload = rawJson
                };
                await _repository.AddAsync(record);
                _logger.LogInformation("Fetched fresh data and saved record at {Timestamp}.", record.Timestamp);

                return new WeatherDto { Timestamp = record.Timestamp, RawJson = rawJson };
            }

            _logger.LogInformation("Falling back to last record.");
            
            var last = await _repository.GetLatestAsync();
            if (last is not null)
                return new WeatherDto { Timestamp = last.Timestamp, RawJson = last.JsonPayload };

            _logger.LogInformation("No data available");
            return null;
        }
    }
}
