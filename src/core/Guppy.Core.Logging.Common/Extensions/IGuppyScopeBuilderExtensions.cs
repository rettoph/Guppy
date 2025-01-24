using Guppy.Core.Common;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Logging.Common.Configurations;
using Guppy.Core.Logging.Common.Constants;

namespace Guppy.Core.Logging.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder ConfigureConsoleLoggerSink(
            this IGuppyScopeBuilder builder,
            string outputTemplate = LoggingConstants.DefaultOutputTemplate,
            bool enabled = true)
        {
            return builder.Configure<ConsoleMessageSinkConfiguration>(conf =>
            {
                conf.OutputTemplate = outputTemplate;
                conf.Enabled = enabled;
            });
        }

        public static IGuppyScopeBuilder ConfigureFileLoggerSink(
            this IGuppyScopeBuilder builder,
            string path,
            string outputTemplate = LoggingConstants.DefaultOutputTemplate,
            bool enabled = true)
        {
            return builder.Configure<FileMessageSinkConfiguration>(conf =>
            {
                conf.Path = path;
                conf.OutputTemplate = outputTemplate;
                conf.Enabled = enabled;
            });
        }
    }
}