using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Logging.Common.Constants;
using Guppy.Game.Common.Configurations;

namespace Guppy.Game.Common.Extensions
{
    public static class IGuppyContainerBuilderExtensions
    {
        public static TBuilder ConfigureTerminalLogMessageSink<TBuilder>(
            this TBuilder builder,
            string outputTemplate = LoggingConstants.DefaultOutputTemplate,
            bool enabled = true)
                where TBuilder : IGuppyContainerBuilder
        {
            builder.Configure<TBuilder, TerminalLogMessageSinkConfiguration>(conf =>
            {
                conf.OutputTemplate = outputTemplate;
                conf.Enabled = enabled;
            });

            return builder;
        }

        public static TBuilder ConfigureTerminalLogMessageSink<TBuilder>(
            this TBuilder builder,
            Action<IGuppyScope, TerminalLogMessageSinkConfiguration> configurator)
                where TBuilder : IGuppyContainerBuilder
        {
            builder.Configure<TBuilder, TerminalLogMessageSinkConfiguration>(configurator);

            return builder;
        }
    }
}