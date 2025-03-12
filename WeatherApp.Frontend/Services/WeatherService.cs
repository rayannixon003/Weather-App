using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WeatherApp.Frontend.Models; // Ensure correct namespace

namespace WeatherApp.Frontend.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private const string BackendUrl = "https://localhost:7078/api/weather"; // ✅ Corrected Backend URL

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = _configuration["OpenWeather:ApiKey"] ?? throw new ArgumentNullException(nameof(_apiKey));
            _baseUrl = "https://api.openweathermap.org/data/2.5/forecast";
        }

        // ✅ Fetch Weather Data from OpenWeather API
        public async Task<WeatherData?> GetWeatherAsync(string city)
        {
            try
            {
                string url = $"{_baseUrl}?q={city}&appid={_apiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error fetching weather data: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<WeatherData>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception fetching weather data: {ex.Message}");
                return null;
            }
        }

        // ✅ Store Weather Data in MongoDB
        public async Task<bool> StoreWeatherDataAsync(WeatherData weatherData)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BackendUrl}/store", weatherData);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception storing weather data: {ex.Message}");
                return false;
            }
        }

        // ✅ Retrieve Stored Historical Weather Data from MongoDB
        public async Task<List<WeatherData>> GetStoredWeatherDataAsync()
        {
            try
            {
                var historicalData = await _httpClient.GetFromJsonAsync<List<HistoricalWeatherModel>>($"{BackendUrl}/get");

                // 🔹 Convert HistoricalWeatherModel to WeatherData
                return historicalData?.Select(h => new WeatherData
                {
                    Id = h.Id,
                    City = new WeatherApp.Frontend.Models.CityInfo // ✅ Fully qualified name to resolve conflict
                    {
                        Name = h.City?.Name ?? "Unknown",
                        Country = h.City?.Country ?? "Unknown"
                    },
                    List = h.List?.Select(item => new WeatherForecastItem
                    {
                        Dt_txt = item.Dt_txt,
                        Main = new MainData
                        {
                            Temp = item.Main?.Temp ?? 0
                        },
                        Weather = item.Weather?.Select(w => new WeatherDescription
                        {
                            Description = w.Description ?? "No description"
                        }).ToList() ?? new List<WeatherDescription>()
                    }).ToList() ?? new List<WeatherForecastItem>()
                }).ToList() ?? new List<WeatherData>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception retrieving weather data: {ex.Message}");
                return new List<WeatherData>();
            }
        }

        // ✅ Delete Stored Weather Data from MongoDB
        public async Task DeleteWeatherDataAsync(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BackendUrl}/delete/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error deleting weather data: {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception deleting weather data: {ex.Message}");
            }
        }
    }
}
