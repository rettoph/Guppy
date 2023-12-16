using Guppy.Resources;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game
{
    public static class Settings
    {
#if DEBUG
        private const LogEventLevel DefaultLogLevel = LogEventLevel.Debug;
#else
        private const LogEventLevel DefaultLogLevel = LogEventLevel.Information;
#endif

        public static Setting<LogEventLevel> LogLevel = Setting.Get<LogEventLevel>(nameof(LogLevel), DefaultLogLevel, "Serilog LogEventLevel. Valid Values: Verbose, Debug, Information, Warning, Fatal.");
    }
}
