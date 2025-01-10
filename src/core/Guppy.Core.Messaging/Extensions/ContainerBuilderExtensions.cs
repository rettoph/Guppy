using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Implementations;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Core.Messaging.Services;

namespace Guppy.Core.Messaging.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCoreMessagingServices(this ContainerBuilder builder)
        {
            if (builder.HasTag(nameof(RegisterCoreMessagingServices)))
            {
                return builder;
            }

            builder.RegisterType<BrokerService>().As<IBrokerService>().InstancePerLifetimeScope();

            builder.RegisterType<Bus>().AsImplementedInterfaces().InstancePerLifetimeScope();

            // TODO: Check if this is used for anything?
            builder.RegisterType<Broker<IMessage>>().As<IBroker<IMessage>>().InstancePerDependency();

            return builder.AddTag(nameof(RegisterCoreMessagingServices));
        }
    }
}