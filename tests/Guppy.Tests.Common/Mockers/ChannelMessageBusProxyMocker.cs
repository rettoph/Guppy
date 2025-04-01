using Guppy.Core.Messaging;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Tests.Common.Proxies;
using Moq;

namespace Guppy.Tests.Common.Mockers
{
    public class ChannelMessageBusProxyMocker : Mocker<ChannelMessageBusProxy>
    {
        public required Mocker<IMessageBusService> MessageBusServiceMocker { get; init; }

        protected override Mock<ChannelMessageBusProxy> BuildMock()
        {
            ChannelMessageBus target = new(this.MessageBusServiceMocker.Object);

            return new Mock<ChannelMessageBusProxy>(target)
            {
                CallBase = true
            };
        }
    }
}
