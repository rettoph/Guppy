using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Logging.Common.Configurations;
using Guppy.Core.Logging.Common.Services;
using Guppy.Core.Logging.Common.Sinks;
using Guppy.Core.Logging.Extensions;
using Guppy.Core.Logging.Serilog.Services;
using Guppy.Core.Logging.Serilog.Sinks;
using Serilog;

namespace Guppy.Core.Logging.Serilog.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterSerilogLoggingServices(this ContainerBuilder builder)
        {
            builder.EnsureRegisteredOnce(nameof(RegisterSerilogLoggingServices), builder =>
            {
                builder.RegisterCoreLoggingServices();
                builder.RegisterType<SerilogLoggerService>().As<ILoggerService>().InstancePerLifetimeScope();

                builder.Configure<LoggerConfiguration>((scope, serilogLoggerConfiguration) =>
                {
                    // Register console sink
                    IConfiguration<ConsoleMessageSinkConfiguration> consoleSinkConfiguration = scope.Resolve<IConfiguration<ConsoleMessageSinkConfiguration>>();
                    if (consoleSinkConfiguration.Value.Enabled == true)
                    {
                        serilogLoggerConfiguration.WriteTo.Console(outputTemplate: consoleSinkConfiguration.Value.OutputTemplate);
                    }

                    // Register file sink
                    IConfiguration<FileMessageSinkConfiguration> fileSinkConfiguration = scope.Resolve<IConfiguration<FileMessageSinkConfiguration>>();
                    if (fileSinkConfiguration.Value.Enabled == true)
                    {
                        serilogLoggerConfiguration.WriteTo.File(
                            path: fileSinkConfiguration.Value.Path ?? throw new Exception(nameof(FileMessageSinkConfiguration.Path)),
                            outputTemplate: fileSinkConfiguration.Value.OutputTemplate,
                            retainedFileCountLimit: 8,
                            shared: true
                        );
                    }

                    // Register ILogMessageSink instances
                    foreach (ILogMessageSink sink in scope.Resolve<IEnumerable<ILogMessageSink>>())
                    {
                        serilogLoggerConfiguration.WriteTo.Sink(new SerilogSink(sink));
                    }
                });
            });

            return builder;
        }
    }
}