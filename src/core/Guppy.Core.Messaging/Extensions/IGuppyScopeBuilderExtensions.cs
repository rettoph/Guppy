using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Implementations;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Core.Messaging.Services;

namespace Guppy.Core.Messaging.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterCoreMessagingServices(this IGuppyScopeBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreMessagingServices), builder =>
            {
                builder.RegisterType<BrokerService>().As<IBrokerService>().InstancePerLifetimeScope();

                builder.RegisterType<Bus>().AsImplementedInterfaces().InstancePerLifetimeScope();

                // TODO: Check if this is used for anything?
                builder.RegisterType<Broker<IMessage>>().As<IBroker<IMessage>>().InstancePerDependency();
            });
        }
    }
}