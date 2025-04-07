using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Logging.Common.Configurations;
using Guppy.Core.Logging.Common.Constants;

namespace Guppy.Core.Logging.Common.Extensions
{
    public static class IGuppyContainerBuilderExtensions
    {
        public static TBuilder ConfigureConsoleLogMessageSink<TBuilder>(
            this TBuilder builder,
            string outputTemplate = LoggingConstants.DefaultOutputTemplate,
            bool enabled = true)
                where TBuilder : IGuppyContainerBuilder
        {
            return builder.Configure<TBuilder, ConsoleLogMessageSinkConfiguration>(conf =>
            {
                conf.OutputTemplate = outputTemplate;
                conf.Enabled = enabled;
            });
        }

        public static TBuilder ConfigureConsoleLogMessageSink<TBuilder>(
            this TBuilder builder,
            Action<IGuppyScope, ConsoleLogMessageSinkConfiguration> configurator)
                where TBuilder : IGuppyContainerBuilder
        {
            return builder.Configure<TBuilder, ConsoleLogMessageSinkConfiguration>(configurator);
        }

        public static TBuilder ConfigureFileLogMessageSink<TBuilder>(
            this TBuilder builder,
            Action<IGuppyScope, FileLogMessageSinkConfiguration> configurator)
                where TBuilder : IGuppyContainerBuilder
        {
            return builder.Configure<TBuilder, FileLogMessageSinkConfiguration>(configurator);
        }

        public static TBuilder ConfigureLogger<TBuilder>(
            this TBuilder builder,
            Action<IGuppyScope, LoggerConfiguration> configurator)
                where TBuilder : IGuppyContainerBuilder
        {
            return builder.Configure<TBuilder, LoggerConfiguration>(configurator);
        }
    }
}