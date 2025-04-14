using MongoDB.Driver;
using BlazorApp.Server.Models;

namespace BlazorApp.Server.Services
{

    public class MongoDBService
    {
        private readonly IMongoDatabase _database;

        public MongoDBService(IConfiguration configuration)
        {
            //var connectionString = configuration.GetConnectionString("MongoDB:ConnectionString");
            var connectionString = configuration.GetConnectionString("MongoDB:ConnectionString");
            // Increase the connection timeout
            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.ConnectTimeout = TimeSpan.FromSeconds(60); // Increase to 60 seconds
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(60); // Increase server selection timeout

            //var client = new MongoClient(connectionString);
            var client = new MongoClient(settings);
            _database = client.GetDatabase("WeatherForecast");
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
}