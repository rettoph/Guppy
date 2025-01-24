using System.Runtime.InteropServices;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Logging.Common.Constants;
using Guppy.Core.Logging.Common.Services;
using Guppy.Core.Logging.Serilog.Extensions;
using Serilog;
using Serilog.Events;
using ILogger = Guppy.Core.Logging.Common.ILogger;

namespace Guppy.Core.Logging.Serilog.Services
{
    public class SerilogLoggerService(ILogLevelService logLevelService, IConfiguration<LoggerConfiguration> configuration) : ILoggerService
    {
        private readonly ILogLevelService _logLevelService = logLevelService;
        private readonly Dictionary<Type, ILogger> _cache = [];
        private readonly ISerilogLogger _base = configuration.Value
            .MinimumLevel.Is(LogEventLevel.Verbose)
            .CreateLogger();

        public ILogger GetLogger(Type context)
        {
            ref ILogger? logger = ref CollectionsMarshal.GetValueRefOrAddDefault(this._cache, context, out bool exists);
            if (exists == true)
            {
                return logger!;
            }

            // If the logger cache does not contain an instance we must create one using reflection

            // First determin the desired log level of the new logger
            LogEventLevel level = this._logLevelService.GetLogLevel(context).ToLogEventLevel();
            ISerilogLogger serilog = new LoggerConfiguration()
                .WriteTo.Logger(this._base)
                .MinimumLevel.Is(level)
                .Enrich.WithProperty(LoggingConstants.SourceContext, context.GetFormattedName())
                .CreateLogger();

            // We make a generic type of SerilogGuppyLogger<context>
            Type loggerType = typeof(SerilogGuppyLogger<>).MakeGenericType(context);
            logger = (ILogger)(Activator.CreateInstance(loggerType, serilog) ?? throw new NotImplementedException());

            // Return the newly created logger
            return logger;
        }
    }
}