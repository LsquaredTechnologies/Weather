using Microsoft.Extensions.Options;

namespace Lsquared.Apps.WeatherForecast;
internal sealed class OpenWeatherMapClient : IWeatherClient
{
    private readonly HttpClient _client;
    private readonly OpenWeatherMapOptions _options;

    public OpenWeatherMapClient(HttpClient client, IOptions<OpenWeatherMapOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<IReadOnlyList<City>> GetCity(string cityNameOrZipCode, string? countryCode = null)
    {
        if (ContainsDigit(cityNameOrZipCode))
        {
            // zip code
            var query = $"zip={cityNameOrZipCode},{countryCode}&appid={_options.ApiKey}";
            var res = await _client.GetFromJsonAsync<GetCityResponse[]>($"/geo/1.0/zip?{query}")!;
            return res.Select(Convert).ToList();
        }
        else
        {
            // name
            var limit = 5;
            var query = $"q={cityNameOrZipCode},{countryCode}&limit={limit}&appid={_options.ApiKey}";
            var res = await _client.GetFromJsonAsync<GetCityResponse[]>($"/geo/1.0/direct?{query}")!;
            return res.Select(Convert).ToList();
        }
    }

    public async Task<CurrentWeather> GetWeather(decimal latitude, decimal longitude)
    {
        var units = "metric";
        var lang = "fr";
        var query = $"lat={latitude}&lon={longitude}&units={units}&lang={lang}&&appid={_options.ApiKey}";
        var res = await _client.GetFromJsonAsync<_CurrentWeather>($"/data/2.5/weather?{query}")!;

        var offset = TimeSpan.FromSeconds(res.Timezone);

        return new()
        {
            City = new()
            {
                Name = res.Name,
                Country = res.Sys.Country,
                Latitude = res.Coord.Lat,
                Longitude = res.Coord.Lon,
            },
            Current = Convert(offset)(res),
            Sunrise = DateTimeOffset.FromUnixTimeSeconds(res.Sys.Sunrise).ToOffset(offset),
            Sunset = DateTimeOffset.FromUnixTimeSeconds(res.Sys.Sunset).ToOffset(offset),
        };
    }

    public async Task<ForecastWeather> GetForecast(decimal latitude, decimal longitude)
    {
        var units = "metric";
        var lang = "fr";
        var query = $"lat={latitude}&lon={longitude}&units={units}&lang={lang}&&appid={_options.ApiKey}";
        var res = await _client.GetFromJsonAsync<_ForecastWeather>($"/data/2.5/forecast?{query}")!;

        var offset = TimeSpan.FromSeconds(res.City.Timezone);

        return new()
        {
            City = new()
            {
                Name = res.City.Name,
                Country = res.City.Country,
                Latitude = res.City.Coord.Lat,
                Longitude = res.City.Coord.Lon,
            },
            Items = res.List.Select(Convert(offset)).ToList()
        };
    }

    private static bool ContainsDigit(string s)
    {
        foreach (var c in s)
        {
            if (c is >= '0' and <= '9')
                return true;
        }

        return false;
    }

    private static City Convert(GetCityResponse res) =>
        new()
        {
            Name = res.Name,
            Country = res.Country,
            Latitude = res.Lat,
            Longitude = res.Lon,
        };

    private static Func<_CurrentWeather, Weather> Convert(TimeSpan offset) => (res) =>
        new Weather()
        {
            Time = DateTimeOffset.FromUnixTimeSeconds(res.Dt).ToOffset(offset),
            Description = res.Weather[0].Description,
            Icon = res.Weather[0].Icon,
            Temperature = new()
            {
                Actual = $"{res.Main.Temp:0} °C",
                FeelsLike = $"{res.Main.Feels_like:0} °C",
            },
            Humidity = new() { Relative = $"{res.Main.Humidity} %" },
            Wind = new() { Speed = $"{res.Wind.Speed} m/s" },
        };

    private sealed class GetCityResponse
    {
        public string? Name { get; init; }
        public string? Country { get; init; }
        public decimal Lat { get; init; }
        public decimal Lon { get; init; }
    }

    private sealed class _CurrentWeather
    {
        public string? Name { get; init; }
        public Coord Coord { get; init; } = new();
        public List<WeatherMain> Weather { get; init; } = new();
        public Temperature Main { get; init; } = new();
        public Wind Wind { get; init; } = new();
        public Sys Sys { get; init; } = new();
        public long Timezone { get; init; }
        public long Dt { get; init; }
    }

    private sealed class _ForecastWeather
    {
        public List<_CurrentWeather> List { get; init; } = new();
        public ForecastCity City { get; init; } = new();
    }

    private sealed class ForecastCity
    {
        public string? Name { get; init; }
        public string? Country { get; init; }
        public long Timezone { get; init; }
        public Coord Coord { get; init; } = new();
    }

    private sealed class WeatherMain
    {
        public string? Main { get; init; }
        public string? Description { get; init; }
        public string? Icon { get; init; }
    }

    private sealed class Temperature
    {
        public decimal Temp { get; init; }
        public decimal Feels_like { get; init; }
        public decimal Humidity { get; init; }
    }

    private sealed class Wind
    {
        public decimal Speed { get; init; }
    }

    private sealed class Coord
    {
        public decimal Lat { get; init; }
        public decimal Lon { get; init; }
    }
    private sealed class Sys
    {
        public string? Country { get; init; }
        public long Sunrise { get; init; }
        public long Sunset { get; init; }
    }
}
