using Autofac;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Commands.Serialization.Commands;
using Guppy.Core.Commands.Services;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;

namespace Guppy.Core.Commands.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterCoreCommandServices(this IGuppyRootBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreCommandServices), builder =>
            {
                builder.RegisterType<CommandService>().AsSelf().As<ICommandService>().InstancePerLifetimeScope();
                builder.RegisterType<CommandTokenService>().As<ICommandTokenService>().InstancePerLifetimeScope();

                builder.RegisterType<NullableEnumTokenConverter>().AsImplementedInterfaces().InstancePerLifetimeScope();
            });
        }
    }
}