using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace WeatherApp.Frontend.Models
{
    public class WeatherData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // ✅ MongoDB Identifier

        public List<WeatherForecastItem> List { get; set; } = new();
        public CityInfo City { get; set; } = new();
    }

    public class WeatherForecastItem
    {
        public long Dt { get; set; } // ✅ Unix timestamp for forecast time
        public MainData Main { get; set; } = new();
        public List<WeatherDescription> Weather { get; set; } = new();
        public string Dt_txt { get; set; } = string.Empty; // ✅ Forecast Date & Time (string format)
    }

    public class MainData
    {
        public float Temp { get; set; }
        public float TempMin { get; set; } // ✅ Minimum temperature
        public float TempMax { get; set; } // ✅ Maximum temperature
    }

    public class WeatherDescription
    {
        public string Main { get; set; } = string.Empty; // ✅ Weather condition (e.g., "Rain", "Clouds")
        public string Description { get; set; } = string.Empty; // ✅ Detailed description (e.g., "light rain")
    }

    public class CityInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}
