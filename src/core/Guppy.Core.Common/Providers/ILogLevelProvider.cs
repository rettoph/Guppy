using Serilog.Events;

namespace Guppy.Core.Common.Providers
{
    public interface ILogLevelProvider
    {
        LogEventLevel? Get(string? context);
    }
}
