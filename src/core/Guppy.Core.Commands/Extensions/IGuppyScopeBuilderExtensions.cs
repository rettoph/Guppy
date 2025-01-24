using Autofac;
using Guppy.Core.Commands.Serialization.Commands;
using Guppy.Core.Commands.Services;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions.Autofac;

namespace Guppy.Core.Commands.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterCoreCommandServices(this IGuppyScopeBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreCommandServices), builder =>
            {
                builder.RegisterType<CommandService>().AsImplementedInterfaces().InstancePerLifetimeScope();
                builder.RegisterType<CommandTokenService>().AsImplementedInterfaces().InstancePerLifetimeScope();

                builder.RegisterType<NullableEnumTokenConverter>().AsImplementedInterfaces().InstancePerLifetimeScope();
            });
        }
    }
}