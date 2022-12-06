
using Microsoft.AspNetCore.Mvc;

namespace Lsquared.Apps.WeatherForecast;
[ApiController]
[Route("weather")]
public sealed partial class WeatherController : ControllerBase
{
    [HttpGet("{cityNameOrZipCode}")]
    public async Task<ActionResult> GetWeather(
        [FromServices] IWeatherClient weatherClient,
        [FromRoute] string cityNameOrZipCode)
    {
        string? countryCode = null;

        var parts = cityNameOrZipCode.Split(',');
        if (parts.Length > 1)
        {
            cityNameOrZipCode = parts[0].Trim();
            countryCode = parts[1].Trim();
        }

        var cities = await weatherClient.GetCity(cityNameOrZipCode, countryCode);

        if (cities.Count is 0)
            return NotFound();

        if (cities.Count is > 1)
            cities = cities.Distinct(CityComparer.Default).ToList();

        if (cities.Count is > 1)
            return Conflict(cities);

        var weather = await weatherClient.GetWeather(cities[0].Latitude, cities[0].Longitude);
        return weather is null ? NotFound() : Ok(weather);
    }

    [HttpGet]
    public async Task<ActionResult> GetWeather(
      [FromServices] IWeatherClient weatherClient,
      [FromQuery(Name = "lat")] decimal latitude,
      [FromQuery(Name = "lng")] decimal longitude)
    {
        var weather = await weatherClient.GetWeather(latitude, longitude);
        return weather is null ? NotFound() : Ok(weather);
    }
}
