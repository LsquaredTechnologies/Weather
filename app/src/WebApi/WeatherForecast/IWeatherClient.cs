namespace Lsquared.Apps.WeatherForecast;

public interface IWeatherClient
{
    Task<IReadOnlyList<City>> GetCity(string cityNameOrZipCode, string? countryCode = null);

    Task<CurrentWeather> GetWeather(decimal latitude, decimal longitude);

    Task<ForecastWeather> GetForecast(decimal latitude, decimal longitude);
}
