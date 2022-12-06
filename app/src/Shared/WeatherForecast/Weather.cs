namespace Lsquared.Apps.WeatherForecast;

public sealed record class Weather
{
    public DateTimeOffset? Time { get; init; }

    public string? Description { get; init; }

    public string? Icon { get; init; }

    public Temperature? Temperature { get; init; }

    public Humidity? Humidity { get; init; }

    public Wind? Wind { get; init; }
}
