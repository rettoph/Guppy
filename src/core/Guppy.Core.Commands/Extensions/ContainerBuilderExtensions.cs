using Autofac;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Commands.Services;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Messaging.Common;
using Guppy.Engine.Common.Autofac;

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


            builder.RegisterType<CommandService>().As<ICommandService>().As<IMagicBroker>().InstancePerMatchingLifetimeScope(LifetimeScopeTags.GuppyScope);

            return builder.AddTag(nameof(RegisterCoreCommandServices));
        }
    }
}
