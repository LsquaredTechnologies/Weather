namespace Lsquared.Apps.WeatherForecast;

public sealed record class Temperature
{
    public string? Actual { get; init; }

    public string? FeelsLike { get; init; }
}
