using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Logging.Common.Enums;
using Guppy.Core.Logging.Modules;
using Guppy.Core.Serialization.Common.Extensions;

namespace Guppy.Core.Logging.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCoreLoggingServices(this ContainerBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreLoggingServices), builder =>
            {
                builder.RegisterModule<LoggingModule>();

                builder.RegisterPolymorphicJsonType<LogLevelEnum, object>(nameof(LogLevelEnum));
            });
        }
    }
}