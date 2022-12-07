using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.FeatureManagement;

namespace Microsoft.AspNetCore.Builder;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureConfiguration(this WebApplicationBuilder builder)
    {
        var appConfigConnectionString = builder.Configuration.GetValue<string>("APPCONFIG_CONNECTIONSTRING");
        appConfigConnectionString ??= builder.Configuration.GetConnectionString("AppConfig");

        if (appConfigConnectionString is null)
        {
            var temporaryServiceProvider = builder.Services.BuildServiceProvider();
            temporaryServiceProvider.GetRequiredService<ILogger<ConfigurationRoot>>().LogAppConfigurationIsNotAvailable();
        }
        else
        {
            var applicationName = builder.Environment.ApplicationName;
            var environmentName = builder.Environment.EnvironmentName;
            var cacheExpiration = builder.Configuration.GetValue("AppConfig:CacheExpiration", TimeSpan.FromMinutes(5));

            var applicationWildcard = string.Concat(applicationName, ":*");
            var applicationPrefix = string.Concat(applicationName, ":");

            builder.Configuration.AddAzureAppConfiguration(
                (options) => options
                    .Connect(appConfigConnectionString)
                    .ConfigureRefresh((refresh) => refresh.Register(string.Concat(applicationName, ":Sentinel"), true).SetCacheExpiration(cacheExpiration))
                    .Select(applicationPrefix, "\0")
                    .Select(applicationPrefix, environmentName)
                    .TrimKeyPrefix(applicationPrefix)
                    .UseFeatureFlags((options) =>
                    {
                        options.CacheExpirationInterval = cacheExpiration;
                        options
                            .Select(applicationWildcard, "\0")
                            .Select(applicationWildcard, environmentName);
                    }),
                optional: true);

            builder.Services.AddAzureAppConfiguration();
        }

        builder.Services.AddFeatureManagement();

        builder.Services.Configure<KestrelServerOptions>((options) =>
        {
            options.AddServerHeader = false;
            //options.Limits.
            options.Limits.MaxConcurrentConnections = 1000;
            options.Limits.MaxConcurrentUpgradedConnections = 1000;
            options.Limits.MaxRequestBodySize = 52428800;
        });

        return builder;
    }
}
