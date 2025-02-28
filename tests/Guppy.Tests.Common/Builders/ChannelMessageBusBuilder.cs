using Guppy.Core.Messaging;
using Guppy.Core.Messaging.Common.Services;

namespace Guppy.Tests.Common.Builders
{
    public class ChannelMessageBusBuilder : Builder<ChannelMessageBus>
    {
        public required Mocker<IMessageBusService> MessageBusServiceMocker { get; init; }

        protected override ChannelMessageBus Build()
        {
            return new ChannelMessageBus(
                messageBusService: this.MessageBusServiceMocker.Object);
        }
    }
}
