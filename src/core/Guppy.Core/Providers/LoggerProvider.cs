using Guppy.Core.Common;
using Guppy.Core.Common.Contexts;
using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Common.Providers;
using Serilog;
using Serilog.Events;
using System.Runtime.InteropServices;

namespace Guppy.Core.Providers
{
    public class LoggerProvider : ILoggerProvider
    {
        private const string SourceContextPropertyName = "SourceContext";

        private readonly ILoggerConfigurationProvider _logLevelService;
        private readonly ILogger _base;
        private readonly ILogger _default;
        private readonly Dictionary<string, ILogger> _loggers;
        private readonly Dictionary<Type, string> _contexts;

        public ILogger Base => _base;
        public ILogger Default => _default;

        public LoggerProvider(
            ILoggerConfigurationProvider logLevelService,
            IConfiguration<LoggerConfiguration> configuration,
            IEnumerable<ServiceLoggerContext> serviceLoggerContexts)
        {
            _loggers = [];
            _logLevelService = logLevelService;
            _base = configuration.Value
                .MinimumLevel.Is(LogEventLevel.Verbose)
                .CreateLogger();
            _default = new LoggerConfiguration()
                .Enrich.WithProperty(SourceContextPropertyName, "Default")
                .MinimumLevel.Is(_logLevelService.TryGetLogLevel(null) ?? throw new NotImplementedException())
                .WriteTo.Logger(_base)
                .CreateLogger();
            _contexts = serviceLoggerContexts.ToDictionary(x => x.ServiceType, x => x.LoggerContext);
        }


        public ILogger Get(Type? contextType)
        {
            if (contextType is null)
            {
                return _default;
            }

            string source = contextType.GetFormattedName();

            ref ILogger? logger = ref CollectionsMarshal.GetValueRefOrAddDefault(_loggers, source, out bool exists);
            if (exists == true)
            {
                return logger!;
            }

            LogEventLevel? level = null;
            if (_contexts.TryGetValue(contextType, out string? context) == true)
            {
                level = _logLevelService.GetOrCreateLogLevel(context);
            }
            else
            {
                context = source;
                level = _logLevelService.TryGetLogLevel(context);
            }

            if (level is null)
            {
                logger = _default.ForContext(SourceContextPropertyName, source);
            }
            else
            {
                logger = new LoggerConfiguration()
                    .MinimumLevel.Is(level.Value)
                    .Enrich.WithProperty(SourceContextPropertyName, source)
                    .WriteTo.Logger(_base)
                    .CreateLogger();
            }

            return logger ?? throw new NotImplementedException();
        }
    }
}
