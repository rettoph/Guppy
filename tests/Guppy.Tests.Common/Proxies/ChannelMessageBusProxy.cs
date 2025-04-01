using Guppy.Core.Messaging;
using Guppy.Core.Messaging.Common;

namespace Guppy.Tests.Common.Proxies
{
    public class ChannelMessageBusProxy(ChannelMessageBus target) : IMessageBus
    {
        public readonly ChannelMessageBus Target = target;

        public virtual void Enqueue<TMessage>(in TMessage message) where TMessage : IMessage
        {
            this.Target.Enqueue<TMessage>(in message);
        }

        public virtual void Flush()
        {
            this.Target.Flush();
        }

        public virtual void Publish<TSequenceGroup, TId, TMessage>(in TId messageId, in TMessage message) where TSequenceGroup : unmanaged, Enum
        {
            this.Target.Publish<TSequenceGroup, TId, TMessage>(in messageId, in message);
        }

        public virtual void Publish<TSequenceGroup, TMessage>(in TMessage message) where TSequenceGroup : unmanaged, Enum
        {
            this.Target.Publish<TSequenceGroup, TMessage>(in message);
        }

        public virtual void Subscribe(object subscriber)
        {
            this.Target.Subscribe(subscriber);
        }

        public virtual void SubscribeAll(IEnumerable<object> subscribers)
        {
            this.Target.SubscribeAll(subscribers);
        }

        public virtual void Unsubscribe(object subscriber)
        {
            this.Target.Unsubscribe(subscriber);
        }

        public virtual void UnsubscribeAll(IEnumerable<object> subscribers)
        {
            this.UnsubscribeAll(subscribers);
        }
    }
}
