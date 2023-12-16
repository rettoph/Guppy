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
        public static Setting<LogEventLevel> LogLevel = Setting.Get<LogEventLevel>(nameof(LogLevel), LogEventLevel.Debug, "Acceptable Values: Verbose, Debug, Information, Warning, Fatal");
#else
        public static Setting<LogEventLevel> LogLevel = Setting.Get<LogEventLevel>(nameof(LogLevel), LogEventLevel.Information, "Acceptable Values: Verbose, Debug, Information, Warning, Fatal");
#endif
    }
}
