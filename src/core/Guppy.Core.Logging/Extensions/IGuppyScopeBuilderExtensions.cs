using Guppy.Core.Common;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Logging.Modules;

namespace Guppy.Core.Logging.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterCoreLoggingServices(this IGuppyScopeBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreLoggingServices), builder =>
            {
                builder.RegisterModule<LoggingModule>();
            });
        }
    }
}