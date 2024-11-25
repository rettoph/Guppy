using Serilog.Events;

namespace Guppy.Core.Common.Providers
{
    public interface ILogLevelService
    {
        LogEventLevel? TryGetLogLevel(string? context);
        LogEventLevel GetOrCreateLogLevel(string context);
    }
}
