using Guppy.Core.Resources.Common;
using Serilog.Events;

namespace Guppy.Game.Common
{
    public static class Settings
    {
#if DEBUG
        private const LogEventLevel DefaultLogLevel = LogEventLevel.Debug;
#else
        private const LogEventLevel DefaultLogLevel = LogEventLevel.Information;
#endif

        public static Setting<LogEventLevel> LogLevel = Setting<LogEventLevel>.Get(nameof(LogLevel), "Serilog LogEventLevel. Valid Values: Verbose, Debug, Information, Warning, Fatal.", DefaultLogLevel);
    }
}
