
using Microsoft.AspNetCore.Mvc;

namespace Lsquared.Apps.WeatherForecast;
[ApiController]
[Route("forecast")]
public sealed partial class ForecastController : ControllerBase
{
    [HttpGet("{cityNameOrZipCode}")]
    public async Task<ActionResult> GetForecast(
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

        var forecast = await weatherClient.GetForecast(cities[0].Latitude, cities[0].Longitude);
        return forecast is null ? NotFound() : Ok(forecast);
    }

    [HttpGet]
    public async Task<ActionResult> GetForecast(
      [FromServices] IWeatherClient weatherClient,
      [FromQuery(Name = "lat")] decimal latitude,
      [FromQuery(Name = "lng")] decimal longitude)
    {
        var forecast = await weatherClient.GetForecast(latitude, longitude);
        return forecast is null ? NotFound() : Ok(forecast);
    }
}
