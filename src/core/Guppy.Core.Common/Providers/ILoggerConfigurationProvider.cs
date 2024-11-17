using Serilog.Events;

namespace Guppy.Core.Common.Providers
{
    public interface ILoggerConfigurationProvider
    {
        LogEventLevel? TryGetLogLevel(string? context);
        LogEventLevel GetOrCreateLogLevel(string context);
    }
}
