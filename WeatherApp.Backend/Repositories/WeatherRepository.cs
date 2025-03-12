using MongoDB.Driver;
using System.Threading.Tasks;
using WeatherApp.Backend.Models;

namespace WeatherApp.Backend.Repositories
{
    public class WeatherRepository
    {
        private readonly IMongoCollection<WeatherData> _weatherCollection;

        public WeatherRepository(IMongoDatabase database)
        {
            _weatherCollection = database.GetCollection<WeatherData>("WeatherData");
        }

        public async Task<bool> InsertWeatherDataAsync(WeatherData weatherData)
        {
            try
            {
                await _weatherCollection.InsertOneAsync(weatherData);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
