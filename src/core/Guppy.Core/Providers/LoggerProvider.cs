using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Common.Extensions.System.Reflection;
using Guppy.Core.Common.Providers;
using Serilog;
using Serilog.Events;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Guppy.Core.Providers
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly ILogLevelProvider _logLevelService;
        private readonly ILogger _base;
        private readonly ILogger _default;
        private readonly Dictionary<string, ILogger> _loggers;

        public ILogger Base => _base;
        public ILogger Default => _default;

        public LoggerProvider(
            ILogLevelProvider logLevelService,
            IConfiguration<LoggerConfiguration> configuration)
        {
            _loggers = [];
            _logLevelService = logLevelService;
            _base = configuration.Value
                .MinimumLevel.Is(LogEventLevel.Verbose)
                .CreateLogger();
            _default = new LoggerConfiguration()
                .Enrich.WithProperty("SourceContext", "Default")
                .MinimumLevel.Is(_logLevelService.Get(null) ?? throw new NotImplementedException())
                .WriteTo.Logger(_base)
                .CreateLogger();
        }


        public ILogger Get(string? context = null)
        {
            if (context is null)
            {
                return _default;
            }

            ref ILogger? logger = ref CollectionsMarshal.GetValueRefOrAddDefault(_loggers, context, out bool exists);
            if (exists == true)
            {
                return logger!;
            }

            LogEventLevel? level = _logLevelService.Get(context);
            if (level is null)
            {
                logger = _default.ForContext("SourceContext", context);
            }
            else
            {
                logger = new LoggerConfiguration()
                    .MinimumLevel.Is(level.Value)
                    .Enrich.WithProperty("SourceContext", context)
                    .WriteTo.Logger(_base)
                    .CreateLogger();
            }

            return logger ?? throw new NotImplementedException();
        }

        public ILogger Get(Type contextType)
        {
            if (contextType.HasCustomAttribute<LoggerContextAttribute>(true) == false)
            {
                return _default.ForContext("SourceContext", contextType.GetFormattedName());
            }

            string? context = contextType.GetCustomAttribute<LoggerContextAttribute>(true)!.Name;
            return this.Get(context);
        }
    }
}
