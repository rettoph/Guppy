using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Files.Common;
using Guppy.Core.Logging.Common.Configurations;
using Guppy.Core.Logging.Common.Constants;

namespace Guppy.Core.Logging.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder ConfigureConsoleLogMessageSink(
            this IGuppyScopeBuilder builder,
            string outputTemplate = LoggingConstants.DefaultOutputTemplate,
            bool enabled = true)
        {
            return builder.Configure<ConsoleLogMessageSinkConfiguration>(conf =>
            {
                conf.OutputTemplate = outputTemplate;
                conf.Enabled = enabled;
            });
        }

        public static IGuppyScopeBuilder ConfigureConsoleLogMessageSink(
            this IGuppyScopeBuilder builder,
            Action<ILifetimeScope, ConsoleLogMessageSinkConfiguration> configurator)
        {
            return builder.Configure<ConsoleLogMessageSinkConfiguration>(configurator);
        }

        public static IGuppyScopeBuilder ConfigureFileLogMessageSink(
            this IGuppyScopeBuilder builder,
            FileLocation path,
            string outputTemplate = LoggingConstants.DefaultOutputTemplate,
            bool enabled = true)
        {
            return builder.Configure<FileLogMessageSinkConfiguration>(conf =>
            {
                conf.Path = path;
                conf.OutputTemplate = outputTemplate;
                conf.Enabled = enabled;
            });
        }

        public static IGuppyScopeBuilder ConfigureFileLogMessageSink(
            this IGuppyScopeBuilder builder,
            Action<ILifetimeScope, FileLogMessageSinkConfiguration> configurator)
        {
            return builder.Configure<FileLogMessageSinkConfiguration>(configurator);
        }

        public static IGuppyScopeBuilder ConfigureLogger(
            this IGuppyScopeBuilder builder,
            Action<ILifetimeScope, LoggerConfiguration> configurator)
        {
            return builder.Configure<LoggerConfiguration>(configurator);
        }
    }
}