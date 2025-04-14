using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using BlazorApp.Server.Models;

namespace BlazorApp.Server.Services
{
    public class WeatherForecastService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public WeatherForecastService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["WeatherAPI:BaseUrl"] ?? "";
            _apiKey = configuration["WeatherAPI:ApiKey"] ?? "";
        }

        public async Task<WeatherData?> GetWeatherAsync(string city)
        {
            if (string.IsNullOrEmpty(city)) return null;

            var url = $"{_baseUrl}?q={city}&appid={_apiKey}&units=metric";
            return await _httpClient.GetFromJsonAsync<WeatherData>(url);
        }
    }
}
