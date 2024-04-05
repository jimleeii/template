using Template.Models;

namespace Template.Services;

public interface IWeatherForecastService
{
    ValueTask<IEnumerable<WeatherForecast>> GetWeatherForecastAsync();
}