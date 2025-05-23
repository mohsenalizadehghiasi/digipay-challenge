namespace Weather.Infrastructure.Providers
{
    public interface IWeatherProvider
    {
        Task<string?> GetRawForecastJsonAsync(CancellationToken cancellation);
    }
}
