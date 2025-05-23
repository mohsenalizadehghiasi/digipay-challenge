using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Weather.Worker
{
    public static class ServiceRegistration
    {
        /// </summary>
        public static IServiceCollection AddWorker(this IServiceCollection services)
        {
            services.AddHostedService<WeatherPoller>();

            return services;
        }

    }
}
