using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Weather.Infrastructure.Persistance.Models;
using Weather.Infrastructure.Persistance.Repositories;
using Weather.Infrastructure.Providers;
using Weather.Worker.Configuration;

namespace Weather.Worker
{
    public class WeatherPoller : BackgroundService
    {
        private readonly IWeatherProvider _provider;
        private readonly IWeatherRepository _repository;
        private readonly ILogger<WeatherPoller> _logger;

        public WeatherPoller(
            IWeatherProvider provider,
            IWeatherRepository repository,
            ILogger<WeatherPoller> logger)
        {
            _provider = provider;
            _repository = repository;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started. Interval: {Interval}s", WorkerConstants.PollIntervalSeconds);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var rawJson = await _provider.GetRawForecastJsonAsync(stoppingToken);
                    if (!string.IsNullOrEmpty(rawJson))
                    {
                        var record = new WeatherRecord
                        {
                            Timestamp = DateTime.UtcNow,
                            JsonPayload = rawJson
                        };
                        await _repository.AddAsync(record);
                        _logger.LogInformation("New data saved at {Time}", record.Timestamp);
                    }
                    else
                    {
                        _logger.LogWarning("No data received.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while polling.");
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(WorkerConstants.PollIntervalSeconds), stoppingToken);
                }
                catch when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Polling delay cancelled"); 
                    break;
                }   
            }

            _logger.LogInformation("Worker stopped.");
        }
    }
}
