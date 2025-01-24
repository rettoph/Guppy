using Autofac;
using Guppy.Core.Logging.Serilog.Extensions;
using Guppy.Engine.Components.Engine;
using Guppy.Engine.Modules;

namespace Guppy.Engine.Extensions.Autofac
{
    internal static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterBootServices(this ContainerBuilder builder)
        {
            builder.RegisterSerilogLoggingServices();

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