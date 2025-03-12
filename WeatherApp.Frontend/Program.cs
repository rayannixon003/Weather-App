using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WeatherApp.Frontend;
using WeatherApp.Frontend.Services;  // ? Keep only ONE instance
using Supabase;
using Microsoft.Extensions.DependencyInjection;
using System;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ? Configure HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7078/") });

#region Register Services

// ? Register WeatherService
builder.Services.AddScoped<WeatherService>();

// ? Register Supabase Authentication Service
var supabaseUrl = "https://qcnicidtrxcwpqpivgyq.supabase.co";
var supabaseApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InFjbmljaWR0cnhjd3BxcGl2Z3lxIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzkxMTQyMDksImV4cCI6MjA1NDY5MDIwOX0.kusfmhvMCDleWAchJVx1bFNCNGQ-Bhv0iLmuyQ4sBIQ"; // Replace with your actual API Key
builder.Services.AddSingleton(new AuthService(supabaseUrl, supabaseApiKey));
// ? Register HttpClient with Base Address
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// ? Register Supabase Client
builder.Services.AddScoped(sp =>
{
    var options = new SupabaseOptions
    {
        AutoConnectRealtime = true,
        AutoRefreshToken = true
    };

    return new Supabase.Client(supabaseUrl, supabaseApiKey, options);
});

#endregion
builder.Services.AddMudServices();


await builder.Build().RunAsync();
