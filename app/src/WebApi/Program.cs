using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// ===== Configures services to the root container ===== //

_ = builder.Services.AddControllersWithViews();
_ = builder.Services.AddRazorPages();
_ = builder.Services.AddEndpointsApiExplorer();
_ = builder.Services.AddSwaggerGen();
_ = builder.Services.AddRateLimiter((options) =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddSlidingWindowLimiter("sliding 15/hour", (options) =>
    {
        options.QueueLimit = 15;
        options.PermitLimit = 15;
        options.Window = TimeSpan.FromHours(1);
    });
    options.AddSlidingWindowLimiter("sliding 300/day", (options) =>
    {
        options.QueueLimit = 300;
        options.PermitLimit = 300;
        options.Window = TimeSpan.FromHours(24);
    });
    options.AddSlidingWindowLimiter("sliding 8000/month", (options) =>
    {
        options.QueueLimit = 8000;
        options.PermitLimit = 8000;
        options.Window = TimeSpan.FromHours(24 * 30);
    });
});

_ = builder.Services.AddHttpClient<IWeatherClient, OpenWeatherMapClient>((HttpClient client) => client.BaseAddress = new("https://api.openweathermap.org/"));
_ = builder.Services.Configure<OpenWeatherMapOptions>(builder.Configuration.GetRequiredSection("OpenWeatherMap"));

var webapp = builder.Build();

// ===== Configures the HTTP request pipeline ===== //

_ = webapp.UseHttpsRedirection();

if (webapp.Environment.IsDevelopment())
{
    _ = webapp.UseDeveloperExceptionPage();
    webapp.UseWebAssemblyDebugging();
}
else
{
    _ = webapp.UseExceptionHandler("/Error");
    _ = webapp.UseHsts(); // TODO change duration
}

if (!webapp.Environment.IsProduction())
    _ = webapp.UseSwaggerUI();

_ = webapp.UseRateLimiter();
_ = webapp.UseSwagger();
_ = webapp.MapControllers();

// Host Blazor client
_ = webapp.UseStaticFiles();
_ = webapp.UseBlazorFrameworkFiles();
_ = webapp.MapRazorPages();
_ = webapp.MapFallbackToFile("index.html");

// ===== Finally, starts the application ===== //

webapp.Run();

internal static partial class Program { }
