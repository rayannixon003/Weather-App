using BlazorApp.Server.Services;
using MongoDB.Driver;

//using BlazorApp;

var builder = WebApplication.CreateBuilder(args);

// Add MongoDB configuration
//builder.Services.Configure<MongoDBSettings>(
//    builder.Configuration.GetSection("MongoDB"));

//builder.Services.AddSingleton<IMongoClient>(sp =>
//    new MongoClient(builder.Configuration["MongoDB:ConnectionString"]));

////mongodb + srv://jaanussrikg:<vjj@AMPA12345>@cluster0.lwgo1.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0

//builder.Services.AddScoped<IMongoDatabase>(sp =>
//    sp.GetRequiredService<IMongoClient>().GetDatabase("WeatherForecast"));



// Add MongoDB configuration
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration["MongoDB:ConnectionString"]));

builder.Services.AddScoped<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase("WeatherForecast"));

// Add MongoDB logging
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Add MongoDB service
builder.Services.AddSingleton<MongoDBService>(); // <-- Add this line


// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()   // Allows requests from any domain
                  .AllowAnyMethod()   // Allows GET, POST, PUT, DELETE, etc.
                  .AllowAnyHeader();  // Allows all headers
        });
});



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient and WeatherForecastService
builder.Services.AddHttpClient<WeatherForecastService>();
//builder.Services.AddScoped<AuthService>();

//// Add MongoDB service
//builder.Services.AddSingleton<MongoDBService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply CORS Policy
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
