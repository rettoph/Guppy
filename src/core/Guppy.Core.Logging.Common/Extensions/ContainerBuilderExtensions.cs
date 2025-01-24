using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Logging.Common.Configurations;
using Guppy.Core.Logging.Common.Constants;

namespace Guppy.Core.Logging.Common.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder ConfigureConsoleLoggerSink(
            this ContainerBuilder builder,
            string outputTemplate = LoggingConstants.DefaultOutputTemplate,
            bool enabled = true)
        {
            return builder.Configure<ConsoleMessageSinkConfiguration>(conf =>
            {
                conf.OutputTemplate = outputTemplate;
                conf.Enabled = enabled;
            });
        }

        public static ContainerBuilder ConfigureFileLoggerSink(
            this ContainerBuilder builder,
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