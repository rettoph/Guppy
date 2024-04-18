using Autofac;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Commands.Services;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Messaging.Common;

namespace Guppy.Core.Commands.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCoreCommandServices(this ContainerBuilder builder)
        {
            if (builder.HasTag(nameof(RegisterCoreCommandServices)))
            {
                return builder;
            }


            builder.RegisterType<CommandService>().As<ICommandService>().As<IMagicBroker>().InstancePerLifetimeScope();

            return builder.AddTag(nameof(RegisterCoreCommandServices));
        }
    }
}
