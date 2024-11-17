using Serilog.Events;

namespace Guppy.Core.Common.Configurations
{
    public class LogLevelConfiguration
    {
#if DEBUG
        public LogEventLevel Default { get; set; } = LogEventLevel.Debug;
#else
        public LogEventLevel Default { get; set; } = LogEventLevel.Information;
#endif

        public Dictionary<string, LogEventLevel?> Contexts { get; set; } = [];
    }
}
