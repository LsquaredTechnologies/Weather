using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// ===== Configures services to the root container ===== //

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// ===== Configures components ===== //

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.RootComponents.RegisterCustomElement<WeatherForeast>("weather-forecast");

// ===== Finally, builds & starts the application ===== //

await builder.Build().RunAsync();

internal static partial class Program { }
