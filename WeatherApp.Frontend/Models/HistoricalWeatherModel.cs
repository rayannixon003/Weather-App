using WeatherApp.Frontend.Models;

public class HistoricalWeatherModel
{
    public string Id { get; set; }  // ✅ Ensure the ID exists
    public CityInfo City { get; set; }  // ✅ Ensure City is an object, not a string
    public List<WeatherForecastItem> List { get; set; }
}

public class CityInfo
{
    public string Name { get; set; }
    public string Country { get; set; }
}
