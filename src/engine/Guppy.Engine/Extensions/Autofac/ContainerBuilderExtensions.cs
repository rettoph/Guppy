using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Engine.Components.Engine;
using Guppy.Engine.Modules;
using Serilog;

namespace Guppy.Engine.Extensions.Autofac
{
    internal static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterBootServices(this ContainerBuilder builder)
        {
            builder.Configure<LoggerConfiguration>((scope, config) =>
            {
                config.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
            });

            return builder;
        }

        public static ContainerBuilder RegisterEngineServices(this ContainerBuilder builder)
        {
            builder.RegisterModule<EngineModule>();

            builder.RegisterType<EngineBrokerComponent>().AsImplementedInterfaces().SingleInstance();

            return builder;
        }
    }
}
