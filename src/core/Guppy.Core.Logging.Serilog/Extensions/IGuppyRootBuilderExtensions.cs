using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Logging.Common.Configurations;
using Guppy.Core.Logging.Common.Enums;
using Guppy.Core.Logging.Common.Services;
using Guppy.Core.Logging.Common.Sinks;
using Guppy.Core.Logging.Extensions;
using Guppy.Core.Logging.Serilog.Services;
using Guppy.Core.Logging.Serilog.Sinks;
using Serilog;
using GuppyLoggerConfiguration = Guppy.Core.Logging.Common.Configurations.LoggerConfiguration;
using SerilogLoggerConfiguration = Serilog.LoggerConfiguration;

namespace Guppy.Core.Logging.Serilog.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterSerilogLoggingServices(this IGuppyRootBuilder builder)
        {
            builder.EnsureRegisteredOnce(nameof(RegisterSerilogLoggingServices), builder =>
            {
                builder.RegisterCoreLoggingServices();
                builder.RegisterType<SerilogLoggerService>().As<ILoggerService>().InstancePerLifetimeScope();

                builder.Configure<SerilogLoggerConfiguration>((scope, serilogLoggerConfiguration) =>
                {
                    // Configure logger
                    IConfiguration<GuppyLoggerConfiguration> guppyloggerConfiguration = scope.Resolve<IConfiguration<GuppyLoggerConfiguration>>();
                    foreach ((string name, object value) in guppyloggerConfiguration.Value.GetEnrichments())
                    {
                        serilogLoggerConfiguration.Enrich.WithProperty(name, value);
                    }

                    foreach ((Type type, LogMessageParameterTypeEnum parameterType) in guppyloggerConfiguration.Value.GetParameterTypes())
                    {
                        switch (parameterType)
                        {
                            case LogMessageParameterTypeEnum.Scalar:
                                serilogLoggerConfiguration.Destructure.AsScalar(type);
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }

                    // Register console sink
                    IConfiguration<ConsoleLogMessageSinkConfiguration> consoleSinkConfiguration = scope.Resolve<IConfiguration<ConsoleLogMessageSinkConfiguration>>();
                    if (consoleSinkConfiguration.Value.Enabled == true)
                    {
                        serilogLoggerConfiguration.WriteTo.Console(outputTemplate: consoleSinkConfiguration.Value.OutputTemplate);
                    }

                    // Register file sink
                    IConfiguration<FileLogMessageSinkConfiguration> fileSinkConfiguration = scope.Resolve<IConfiguration<FileLogMessageSinkConfiguration>>();
                    if (fileSinkConfiguration.Value.Enabled == true)
                    {
                        serilogLoggerConfiguration.WriteTo.File(
                            path: fileSinkConfiguration.Value.Path?.Path ?? throw new Exception(nameof(FileLogMessageSinkConfiguration.Path)),
                            outputTemplate: fileSinkConfiguration.Value.OutputTemplate,
                            retainedFileCountLimit: 8,
                            shared: true
                        );
                    }

                    // Register ILogMessageSink instances
                    foreach (ILogMessageSink sink in scope.Resolve<IEnumerable<ILogMessageSink>>())
                    {
                        if (sink.Enabled == true)
                        {
                            serilogLoggerConfiguration.WriteTo.Sink(new SerilogSink(sink));
                        }
                    }
                });
            });

            return builder;
        }
    }
}