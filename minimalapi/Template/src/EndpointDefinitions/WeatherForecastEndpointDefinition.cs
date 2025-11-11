using EndpointDefinition;

namespace Template.EndpointDefinitions;

/// <summary>
/// The weather forecast endpoint definition.
/// </summary>
public class WeatherForecastEndpointDefinition : IEndpointDefinition
{
    /// <summary>
    /// Defines the endpoints.
    /// </summary>
    /// <param name="app">The app.</param>
    /// <param name="env">The environment.</param>
    public void DefineEndpoints(WebApplication app, IWebHostEnvironment env)
    {
        app.MapGet("api/WeatherForecast", WeatherForecastAsync);
    }

    /// <summary>
    /// Defines the services.
    /// </summary>
    /// <param name="services">The services.</param>
    public void DefineServices(IServiceCollection services)
    {
        services.AddSingleton<IWeatherForecastService, WeatherForecastService>();
    }

    /// <summary>
    /// Weathers the forecast asynchronously.
    /// </summary>
    /// <param name="service">The weather forecast service.</param>
    /// <returns>A Task of type IResult</returns>
    private async Task<IResult> WeatherForecastAsync(IWeatherForecastService service)
    {
        var forecasts = await service.GetWeatherForecastAsync();
        return Results.Ok(forecasts);
    }
}