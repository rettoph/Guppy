using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Logging.Serilog.Extensions;
using Guppy.Engine.Modules;

namespace Guppy.Engine.Extensions
{
    internal static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterBootServices(this IGuppyRootBuilder builder)
        {
            builder.RegisterSerilogLoggingServices();

            return builder;
        }

        public static IGuppyRootBuilder RegisterEngineServices(this IGuppyRootBuilder builder)
        {
            builder.RegisterModule<EngineModule>();

            return builder;
        }
    }
}