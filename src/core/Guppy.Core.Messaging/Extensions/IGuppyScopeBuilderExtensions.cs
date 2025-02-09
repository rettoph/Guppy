using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Core.Messaging.Services;
using Guppy.Core.Messaging.Systems.Global;

namespace Guppy.Core.Messaging.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterCoreMessagingServices(this IGuppyScopeBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreMessagingServices), builder =>
            {
                builder.RegisterGlobalSystem<AutoSubscribeGlobalSystemsToBrokerServiceSystem>();
                builder.RegisterScopedSystem<AutoSubscribeScopedSystemsToBrokerServiceSystem>();

                builder.Register(ctx => new MessageBusService(x => new ChannelMessageBus(x))).As<IMessageBusService>().SingleInstance();
                builder.Register(ctx => ctx.Resolve<IMessageBusService>().Create()).As<IMessageBus>().InstancePerLifetimeScope();
            });
        }
    }
}