using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp.Backend.Models;
using WeatherApp.Backend.Services;

namespace WeatherApp.Backend.Controllers
{
    [Route("api/historicalweather")]
    [ApiController]
    public class HistoricalWeatherController : ControllerBase
    {
        private readonly HistoricalWeatherService _historicalWeatherService;

        public HistoricalWeatherController(HistoricalWeatherService historicalWeatherService)
        {
            _historicalWeatherService = historicalWeatherService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveHistoricalWeather([FromBody] HistoricalWeatherModel weatherData)
        {
            await _historicalWeatherService.SaveHistoricalWeatherData(weatherData);
            return Ok(new { message = "Historical weather data saved successfully!" });
        }

        [HttpGet("get")]
        public async Task<ActionResult<List<HistoricalWeatherModel>>> GetHistoricalWeather()
        {
            var data = await _historicalWeatherService.GetHistoricalWeather();
            return Ok(data);
        }
    }
}
