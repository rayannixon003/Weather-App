using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherApp.Backend.Services;

namespace WeatherApp.Backend.Controllers
{
    [Route("api/weather")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetWeather([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City parameter is required.");
            }

            var weatherData = await _weatherService.GetWeatherAsync(city);
            if (weatherData == null)
            {
                return NotFound("Weather data not found.");
            }

            return Ok(weatherData);
        }
    }
}
