using System.Diagnostics.CodeAnalysis;

namespace Lsquared.Apps.WeatherForecast;

public sealed record class City
{
    [NotNull]
    public string? Name { get; init; }

    [NotNull]
    public string? Country { get; init; }

    public decimal Latitude { get; init; }

    public decimal Longitude { get; init; }
}
