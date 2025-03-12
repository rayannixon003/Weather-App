using WeatherApp.Backend.Services;
using WeatherApp.Backend.Models;
using MongoDB.Driver;
using WeatherApp.Backend.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// ✅ Print all configurations for debugging
Console.WriteLine("🔍 Debug: Loading appsettings.json...");
foreach (var kvp in builder.Configuration.AsEnumerable())
{
    Console.WriteLine($"➡️ {kvp.Key}: {kvp.Value}");
}

// ✅ Read MongoDB Configuration from `appsettings.json`
var mongoSettings = builder.Configuration.GetSection("MongoDB");
var mongoConnectionString = mongoSettings.GetValue<string>("ConnectionString");
var mongoDatabaseName = mongoSettings.GetValue<string>("DatabaseName");

// ✅ Debugging output for verification
Console.WriteLine($"🔹 MongoDB Connection String: {mongoConnectionString}");
Console.WriteLine($"🔹 MongoDB Database Name: {mongoDatabaseName}");

if (string.IsNullOrWhiteSpace(mongoConnectionString))
{
    throw new ArgumentNullException(nameof(mongoConnectionString), "MongoDB connection string is missing in appsettings.json!");
}

if (string.IsNullOrWhiteSpace(mongoDatabaseName))
{
    throw new ArgumentNullException(nameof(mongoDatabaseName), "MongoDB database name is missing in appsettings.json!");
}

// ✅ Add MongoDB Client and Database to DI
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDatabaseName);
});

// ✅ Register Repositories & Services
builder.Services.AddSingleton<HistoricalWeatherRepository>();
builder.Services.AddScoped<HistoricalWeatherService>();

// ✅ Add WeatherService with HttpClient (Only Once)
builder.Services.AddHttpClient<WeatherService>();

// ✅ Add Controllers & Swagger (API Documentation)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("https://localhost:7010")  // Allow frontend URL
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// ✅ Ensure `Build()` is Called Only Once
var app = builder.Build();

// ✅ Enable CORS
app.UseCors("AllowFrontend");  // Apply CORS Policy

// ✅ Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ✅ Run the App (Only Once!)
app.Run();
