using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Template.Tools;

/// <summary>
/// The stock query tool.
/// </summary>
[McpServerToolType]
public static class StockQueryTool
{
    /// <summary>
    /// Queries weather forecast data asynchronously and returns the results back to the client.
    /// The data returned is the next 5 days of weather forecast data.
    /// </summary>
    /// <param name="weatherForecastService">The weather forecast service used to fetch weather forecast data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of WeatherForecast.</returns>
    [McpServerTool, Description("Queries weather forecast data and returns the next 5 days of results back to the client.")]
    public static async Task<IEnumerable<WeatherForecast>> QueryAsync(
        IWeatherForecastService weatherForecastService,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<WeatherForecast> data = await weatherForecastService.GetWeatherForecastAsync(cancellationToken);
        return data.Take(5);
    }

    /// <summary>
    /// Queries weather forecast data and returns the results back to the client.
    /// The data returned is the next <paramref name="days"/> days of weather forecast data.
    /// </summary>
    /// <param name="weatherForecastService">The weather forecast service used to fetch weather forecast data.</param>
    /// <param name="days">The number of days of weather forecast data to return.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of WeatherForecast.</returns>
    [McpServerTool, Description("Queries weather forecast data and returns the results back to the client.")]
    public static async Task<IEnumerable<WeatherForecast>> QueryByDaysAsync(
        IWeatherForecastService weatherForecastService,
        [Description("The number of days of weather forecast data to return.")] int days,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<WeatherForecast> data = await weatherForecastService.GetWeatherForecastByDaysAsync(days, cancellationToken);
        return data;
    }
}