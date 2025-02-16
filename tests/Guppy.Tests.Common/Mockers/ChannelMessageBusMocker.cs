using Guppy.Core.Messaging;
using Guppy.Core.Messaging.Services;
using Guppy.Tests.Common.Mocks;

namespace Guppy.Tests.Common.Mockers
{
    public class ChannelMessageBusMocker : BaseMockerBuilder<ChannelMessageBus>
    {
        public Mocker<MessageBusService> MessageBusServiceMocker { get; set; } = new();

        public ChannelMessageBus ChannelMessageBus => this.GetInstance();

        protected override ChannelMessageBus Build()
        {
            return new ChannelMessageBus(this.MessageBusServiceMocker.GetInstance());
        }
    }
}
