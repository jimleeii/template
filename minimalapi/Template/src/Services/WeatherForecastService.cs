using Template.Models;

namespace Template.Services;

/// <summary>
/// The weather forecast service.
/// </summary>
public class WeatherForecastService : IWeatherForecastService
{
    /// <summary>
    /// The summaries.
    /// </summary>
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    /// <summary>
    /// Get weather forecast asynchronously.
    /// </summary>
    /// <returns>A ValueTask of a list of weather forecasts</returns>
    public ValueTask<IEnumerable<WeatherForecast>> GetWeatherForecastAsync()
    {
        return ValueTask.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }));
    }
}