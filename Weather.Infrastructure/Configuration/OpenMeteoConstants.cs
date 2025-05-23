
namespace Weather.Infrastructure.Configuration
{
    public class OpenMeteoConstants
    {
        public const string BaseUrl = "https://api.open-meteo.com/v1/forecast";
        public const double DefaultLatitude = 52.52;
        public const double DefaultLongitude = 13.41;
        public const int TimeoutSeconds = 2;
        public const int RetryCount = 3;
    }
}
