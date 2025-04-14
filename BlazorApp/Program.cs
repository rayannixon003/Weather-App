using BlazorApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BlazorApp.Services;
using Supabase;
//using Supabase.Gotrue;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//// Register HttpClient
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register HttpClient with the correct base address
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7124") // Update this to match your backend URL
});

// Register WeatherService
builder.Services.AddScoped<GetWeatherService>();

builder.Services.AddScoped(provider =>
    new Supabase.Client(
        "https://smgnltgcikdgconfjegi.supabase.co",
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNtZ25sdGdjaWtkZ2NvbmZqZWdpIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDE0NTQ0ODAsImV4cCI6MjA1NzAzMDQ4MH0.HbjqQUpbn99U2J6OivAJ8fimNXxIY3VYKStmxs5bBpM",
        new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }
    )
);

// Register AuthService
builder.Services.AddScoped<AuthService>();

//// Add MongoDB service
//builder.Services.AddSingleton<MongoDBService>();

await builder.Build().RunAsync();