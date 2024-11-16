using Serilog;
using Serilog.Events;

namespace Guppy.Engine.Common.Providers
{
    public interface ILogLevelProvider
    {
        void Configure(LoggerConfiguration configuration);

        LogEventLevel Get(string? context = null, LogEventLevel? defaultLevel = null);
    }
}
