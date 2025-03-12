using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using WeatherApp.Backend.Models;

namespace WeatherApp.Backend.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly IMongoCollection<WeatherData> _weatherCollection;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeather:ApiKey"] ?? throw new ArgumentNullException(nameof(_apiKey), "OpenWeather API key is missing.");
            _baseUrl = configuration["OpenWeather:BaseUrl"] ?? throw new ArgumentNullException(nameof(_baseUrl), "OpenWeather BaseUrl is missing.");

            // ✅ Correct Order: Assign values before using them
            var mongoConfig = configuration.GetSection("MongoDB");

            string connectionString = mongoConfig["ConnectionString"] ?? throw new ArgumentNullException(nameof(connectionString), "MongoDB connection string is missing.");
            string databaseName = mongoConfig["DatabaseName"] ?? throw new ArgumentNullException(nameof(databaseName), "MongoDB database name is missing.");
            string collectionName = mongoConfig["CollectionName"] ?? throw new ArgumentNullException(nameof(collectionName), "MongoDB collection name is missing.");

            // ✅ Initialize MongoDB Client
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _weatherCollection = database.GetCollection<WeatherData>(collectionName);
        }

        // ✅ Fetch Weather Data from OpenWeather API
        public async Task<string?> GetWeatherAsync(string city)
        {
            try
            {
                string url = $"{_baseUrl}?q={city}&appid={_apiKey}&units=metric";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadAsStringAsync(); // ✅ Return JSON response
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching weather data: {ex.Message}");
                return null;
            }
        }

        // ✅ Store Weather Data in MongoDB
        public async Task<bool> StoreWeatherDataAsync(WeatherData weatherData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(weatherData.City))
                {
                    Console.WriteLine("Invalid weather data: Missing city name.");
                    return false;
                }

                await _weatherCollection.InsertOneAsync(weatherData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception storing weather data: {ex.Message}");
                return false;
            }
        }
    }
}
