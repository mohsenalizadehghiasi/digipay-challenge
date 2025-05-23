using Weather.Application.Interfaces;
using Weather.Application.Services;
using Microsoft.Extensions.DependencyInjection;
namespace Weather.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IWeatherService, WeatherService>();
            return services;
        }
    }
}
