
using System.Diagnostics.CodeAnalysis;

namespace Lsquared.Apps.WeatherForecast;
internal sealed class CityComparer : IEqualityComparer<City>
{
    public static readonly CityComparer Default = new();

    public bool Equals(City? x, City? y) =>
        x?.Name == y?.Name && x?.Country == y?.Country;

    public int GetHashCode([DisallowNull] City obj) =>
        HashCode.Combine(obj.Name, obj.Country);
}
