using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WeatherApp.Backend.Models
{
    public class HistoricalWeatherModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("location")]
        public string Location { get; set; } = string.Empty;

        [BsonElement("temperature")]
        public double Temperature { get; set; }

        [BsonElement("humidity")]
        public int Humidity { get; set; }

        [BsonElement("weatherDescription")]
        public string WeatherDescription { get; set; } = string.Empty;

        [BsonElement("date")]
        public DateTime Date { get; set; }
    }
}
