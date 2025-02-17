using Guppy.Core.Messaging;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Services;
using Moq;

namespace Guppy.Tests.Common.Mockers
{
    public class ChannelMessageBusProxyMocker
    {
        public Mocker<IMessageBusService> MessageBusServiceMocker { get; set; }
        public ChannelMessageBus ChannelMessageBusProxy { get; set; }
        public Mocker<IMessageBus> MessageBusMocker { get; set; }

        public ChannelMessageBusProxyMocker()
        {
            this.MessageBusServiceMocker = new Mocker<IMessageBusService>();
            this.ChannelMessageBusProxy = new ChannelMessageBus(this.MessageBusServiceMocker.GetInstance());
            this.MessageBusMocker = new Mocker<IMessageBus>();

            this.MessageBusMocker.Setup(
                expression: x => x.Flush(),
                callback: () => this.ChannelMessageBusProxy.Flush());

            this.MessageBusMocker.Setup<object>(
                expression: x => x.Subscribe(It.IsAny<object>()),
                callback: x => this.ChannelMessageBusProxy.Subscribe(x));

            this.MessageBusMocker.Setup<IEnumerable<object>>(
                expression: x => x.SubscribeAll(It.IsAny<IEnumerable<object>>()),
                callback: x => this.ChannelMessageBusProxy.SubscribeAll(x));

            this.MessageBusMocker.Setup<object>(
                expression: x => x.Unsubscribe(It.IsAny<object>()),
                callback: x => this.ChannelMessageBusProxy.Unsubscribe(x));

            this.MessageBusMocker.Setup<IEnumerable<object>>(
                expression: x => x.UnsubscribeAll(It.IsAny<IEnumerable<object>>()),
                callback: x => this.ChannelMessageBusProxy.UnsubscribeAll(x));
        }

        public ChannelMessageBusProxyMocker ProxyPublish<TSequenceGroup, TId, TMessage>()
            where TSequenceGroup : unmanaged, Enum
        {
            this.MessageBusMocker.Setup<TId, TMessage>(
                expression: x => x.Publish<TSequenceGroup, TId, TMessage>(It.Ref<TId>.IsAny, It.Ref<TMessage>.IsAny),
                callback: (id, message) => this.ChannelMessageBusProxy.Publish<TSequenceGroup, TId, TMessage>(id, message));

            return this;
        }

        public ChannelMessageBusProxyMocker ProxyPublish<TSequenceGroup, TMessage>()
            where TSequenceGroup : unmanaged, Enum
        {
            this.MessageBusMocker.Setup(
                expression: x => x.Publish<TSequenceGroup, TMessage>(It.Ref<TMessage>.IsAny),
                callback: (in TMessage message) => this.ChannelMessageBusProxy.Publish<TSequenceGroup, TMessage>(message));

            return this;
        }

        public ChannelMessageBusProxyMocker ProxyEnqueue<TMessage>()
            where TMessage : IMessage
        {
            this.MessageBusMocker.Setup<TMessage>(
                expression: x => x.Enqueue<TMessage>(It.Ref<TMessage>.IsAny),
                callback: (message) => this.ChannelMessageBusProxy.Enqueue<TMessage>(message));

            return this;
        }
    }
}
