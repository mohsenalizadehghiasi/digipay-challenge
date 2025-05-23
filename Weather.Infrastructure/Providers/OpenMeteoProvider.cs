using Weather.Infrastructure.Configuration;

namespace Weather.Infrastructure.Providers
{
    public class OpenMeteoProvider : IWeatherProvider
    {
        private readonly HttpClient _http;
        public OpenMeteoProvider(HttpClient http)
        {
            _http = http;
        }

        public async Task<string?> GetRawForecastJsonAsync(CancellationToken cancellation)
        {
            var url = $"?latitude={OpenMeteoConstants.DefaultLatitude}&longitude={OpenMeteoConstants.DefaultLongitude}&hourly=temperature_2m";
            using var response = await _http.GetAsync(url, cancellation);
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadAsStringAsync(cancellation);
        }
    }
}
