namespace Lsquared.Apps.WeatherForecast;

public sealed record class ForecastWeather
{
    public City City { get; init; } = new();
    public List<Weather> Items { get; init; } = new();
}
