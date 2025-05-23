using Microsoft.AspNetCore.Mvc;
using Weather.Application.Dtos;
using Weather.Application.Interfaces;

namespace Weather.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }


        public async Task<IActionResult> Get(CancellationToken ct)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(5));

            WeatherDto? result;
            try
            {
                result = await _weatherService.GetLiveOrCachedAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(504, "Request timed out.");
            }

            if (result == null)
                return NoContent();

            return Ok(result);
        }
    }
}
