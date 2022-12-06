namespace Lsquared.Apps.WeatherForecast;

public sealed record class CurrentWeather
{
    public City? City { get; init; }

    public Weather? Current { get; init; }

    public DateTimeOffset? Sunrise { get; init; }

    public DateTimeOffset? Sunset { get; init; }
}
