using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Weather.Infrastructure.Configuration;
using Weather.Infrastructure.Persistance;
using Weather.Infrastructure.Persistance.Repositories;
using Weather.Infrastructure.Providers;

namespace Weather.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<WeatherDbContext>(opts => opts.UseSqlServer("Server=ServerAddress;Database=Weather;TrustServerCertificate=True;"));

            services.AddScoped<IWeatherRepository, WeatherRepository>();

            services.AddHttpClient<IWeatherProvider, OpenMeteoProvider>(client =>
            {
                client.BaseAddress = new Uri(config.GetValue<string>("OpenMeteo:BaseUrl"));
                client.Timeout = TimeSpan.FromSeconds(config.GetValue<int>("OpenMeteo:TimeoutSeconds"));
            })
            .AddTransientHttpErrorPolicy(policy => policy
                .WaitAndRetryAsync(
                    retryCount: config.GetValue<int>("OpenMeteo:RetryCount"),
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                ));

            return services;
        }
    }
}
