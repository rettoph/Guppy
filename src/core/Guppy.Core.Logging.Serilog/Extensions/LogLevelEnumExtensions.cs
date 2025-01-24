using System.Diagnostics;
using Guppy.Core.Logging.Common.Enums;
using Serilog.Events;

namespace Guppy.Core.Logging.Serilog.Extensions
{
    public static class LogLevelEnumExtensions
    {
        public static LogEventLevel ToLogEventLevel(this LogLevelEnum logLevel)
        {
            return logLevel switch
            {
                LogLevelEnum.Verbose => LogEventLevel.Verbose,
                LogLevelEnum.Debug => LogEventLevel.Debug,
                LogLevelEnum.Information => LogEventLevel.Information,
                LogLevelEnum.Warning => LogEventLevel.Warning,
                LogLevelEnum.Error => LogEventLevel.Error,
                LogLevelEnum.Fatal => LogEventLevel.Fatal,
                _ => throw new UnreachableException()
            };
        }

        public static LogLevelEnum ToLogLevelEnum(this LogEventLevel logLevel)
        {
            return logLevel switch
            {
                LogEventLevel.Verbose => LogLevelEnum.Verbose,
                LogEventLevel.Debug => LogLevelEnum.Debug,
                LogEventLevel.Information => LogLevelEnum.Information,
                LogEventLevel.Warning => LogLevelEnum.Warning,
                LogEventLevel.Error => LogLevelEnum.Error,
                LogEventLevel.Fatal => LogLevelEnum.Fatal,
                _ => throw new UnreachableException()
            };
        }
    }
}