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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A ValueTask of a list of weather forecasts</returns>
    public ValueTask<IEnumerable<WeatherForecast>> GetWeatherForecastAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return ValueTask.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }));
    }

    /// <summary>
    /// Get weather forecast by days asynchronously.
    /// </summary>
    /// <param name="days">The query parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A ValueTask of a list of weather forecasts</returns>
    public ValueTask<IEnumerable<WeatherForecast>> GetWeatherForecastByDaysAsync(int days, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return ValueTask.FromResult(Enumerable.Range(1, days).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }));
    }
}