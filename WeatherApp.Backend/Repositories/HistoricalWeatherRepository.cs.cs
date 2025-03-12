using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp.Backend.Models;

namespace WeatherApp.Backend.Repositories
{
    public class HistoricalWeatherRepository
    {
        private readonly IMongoCollection<HistoricalWeatherModel> _weatherCollection;

        public HistoricalWeatherRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("WeatherDB");
            _weatherCollection = database.GetCollection<HistoricalWeatherModel>("HistoricalWeather");
        }

        public async Task InsertWeatherData(HistoricalWeatherModel weatherData)
        {
            await _weatherCollection.InsertOneAsync(weatherData);
        }

        public async Task<List<HistoricalWeatherModel>> GetHistoricalWeather()
        {
            return await _weatherCollection.Find(_ => true).ToListAsync();
        }
    }
}
