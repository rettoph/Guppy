using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.Logging
{
    public static class ILoggerExtensions
    {
        public static void Log(this ILogger logger, LogLevel level, Func<String> message)
        {
            if (logger.IsEnabled(level))
                logger.Log(level, message());
        }

        public static void LogInformation(this ILogger logger, Func<String> message)
        {
            logger.Log(LogLevel.Information, message);
        }

        public static void LogWarning(this ILogger logger, Func<String> message)
        {
            logger.Log(LogLevel.Warning, message);
        }

        public static void LogTrace(this ILogger logger, Func<String> message)
        {
            logger.Log(LogLevel.Trace, message);
        }

        public static void LogError(this ILogger logger, Func<String> message)
        {
            logger.Log(LogLevel.Error, message);
        }

        public static void LogDebug(this ILogger logger, Func<String> message)
        {
            logger.Log(LogLevel.Debug, message);
        }

        public static void LogCritical(this ILogger logger, Func<String> message)
        {
            logger.Log(LogLevel.Critical, message);
        }
    }
}
