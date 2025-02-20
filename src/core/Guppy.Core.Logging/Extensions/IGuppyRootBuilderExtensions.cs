using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Logging.Common;
using Guppy.Core.Logging.Common.Enums;
using Guppy.Core.Logging.Common.Extensions;
using Guppy.Core.Logging.Common.Services;
using Guppy.Core.Logging.Modules;
using Guppy.Core.Logging.Services;
using Guppy.Core.Serialization.Common.Extensions;

namespace Guppy.Core.Logging.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterCoreLoggingServices(this IGuppyRootBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreLoggingServices), builder =>
            {
                builder.RegisterType<LogLevelService>().As<ILogLevelService>().SingleInstance();

                builder.RegisterPolymorphicJsonType<LogLevelEnum, object>(nameof(LogLevelEnum));

                builder.RegisterGeneric((context, types) => context.ResolveLogger(types[0])).As(typeof(ILogger<>));
                builder.Register(context => context.ResolveLogger<ILogger>()).As<ILogger>();

                builder.RegisterModule<LoggingModule>();
            });
        }
    }
}