using System.Diagnostics.CodeAnalysis;

namespace Lsquared.Apps.WeatherForecast;

public sealed record class Country
{
    [NotNull]
    public string? Code { get; init; }
}
