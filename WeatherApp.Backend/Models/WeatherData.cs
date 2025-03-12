using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace WeatherApp.Backend.Models
{
    public class WeatherData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string City { get; set; }
        public double Temperature { get; set; }
        public string Condition { get; set; }
        public string Date { get; set; }
    }
}
