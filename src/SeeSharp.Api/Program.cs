using Microsoft.EntityFrameworkCore;
using SeeSharp.Api;

// It doesn't have to be called Program.cs, but that's the convention.

// APIs are made using the builder pattern!
var apiBuilder = WebApplication.CreateBuilder(args);

// Register the DbContext (scoped per request)
apiBuilder.Services.AddDbContext<SeeSharpContext>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
apiBuilder.Services.AddOpenApi();

// Redis
apiBuilder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = apiBuilder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
    options.InstanceName = "SeeSharp_";
});

var app = apiBuilder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPatientEndpoints();
// Copilot added MapPatientEndpoints, which tells me we also need MapGlassesEndpoints, etc.
app.MapGlassesEndPoints();
app.MapAppointmentEndpoints();

/***** Run the application *****/

app.Run();

/**** Record declarations ****/

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
