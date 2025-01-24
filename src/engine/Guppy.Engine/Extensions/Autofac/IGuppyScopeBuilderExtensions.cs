using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Logging.Serilog.Extensions;
using Guppy.Engine.Components.Engine;
using Guppy.Engine.Modules;

namespace Guppy.Engine.Extensions.Autofac
{
    internal static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterBootServices(this IGuppyScopeBuilder builder)
        {
            builder.RegisterSerilogLoggingServices();

            return builder;
        }

        public static IGuppyScopeBuilder RegisterEngineServices(this IGuppyScopeBuilder builder)
        {
            builder.RegisterModule<EngineModule>();

            builder.RegisterType<EngineBrokerComponent>().AsImplementedInterfaces().SingleInstance();

            return builder;
        }
    }
}