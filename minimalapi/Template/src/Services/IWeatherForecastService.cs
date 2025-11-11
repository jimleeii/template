namespace Template.Services;

public interface IWeatherForecastService
{
    ValueTask<IEnumerable<WeatherForecast>> GetWeatherForecastAsync(CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<WeatherForecast>> GetWeatherForecastByDaysAsync(int days, CancellationToken cancellationToken = default);
}