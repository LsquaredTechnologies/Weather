namespace Microsoft.AspNetCore.Builder;

internal static partial class AzureAppConfigurationLoggerExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Azure AppConfiguration is not available. Specify connection string if needed")]
    internal static partial void LogAppConfigurationIsNotAvailable(this ILogger logger);
}
