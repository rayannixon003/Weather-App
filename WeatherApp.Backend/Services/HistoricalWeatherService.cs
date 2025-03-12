using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp.Backend.Models;
using WeatherApp.Backend.Repositories;

namespace WeatherApp.Backend.Services
{
    public class HistoricalWeatherService
    {
        private readonly HistoricalWeatherRepository _weatherRepository;

        public HistoricalWeatherService(HistoricalWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        public async Task SaveHistoricalWeatherData(HistoricalWeatherModel weatherData)
        {
            await _weatherRepository.InsertWeatherData(weatherData);
        }

        public async Task<List<HistoricalWeatherModel>> GetHistoricalWeather()
        {
            return await _weatherRepository.GetHistoricalWeather();
        }
    }
}
