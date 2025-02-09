using Guppy.Core.Common;
using Guppy.Core.Messaging.Common;

namespace Guppy.Core.Messaging.Utilities
{
    public delegate void IdMessagePublishDelegate<TId, TMessage>(in TId id, TMessage message);
    public class MessagePublisher<TSequenceGroup, TId, TMessage> : DelegateSequenceGroup<TSequenceGroup, IdMessagePublishDelegate<TId, TMessage>>, IMessagePublisher
        where TSequenceGroup : unmanaged, Enum
    {
        public MessagePublisher() : base(null, false)
        {
        }

        public void Publish(in TId id, in TMessage message)
        {
            this.Sequenced?.Invoke(in id, message);
        }

        public void Subscribe(object instance)
        {
            this.Add(instance.Yield());
        }

        public void Subscribe(IEnumerable<object> instances)
        {
            this.Add(instances);
        }

        public void Unsubscribe(object instance)
        {
            this.Remove(instance.Yield());
        }

        public void Unsubscribe(IEnumerable<object> instances)
        {
            this.Remove(instances);
        }
    }

    public delegate void MessagePublishDelegate<TMessage>(TMessage message);
    public class MessagePublisher<TSequenceGroup, TMessage> : DelegateSequenceGroup<TSequenceGroup, MessagePublishDelegate<TMessage>>, IMessagePublisher
        where TSequenceGroup : unmanaged, Enum
    {
        public MessagePublisher() : base(null, false)
        {
        }

        public void Publish(in TMessage message)
        {
            this.Sequenced?.Invoke(message);
        }

        public void Subscribe(object instance)
        {
            this.Add(instance.Yield());
        }

        public void Subscribe(IEnumerable<object> instances)
        {
            this.Add(instances);
        }

        public void Unsubscribe(object instance)
        {
            this.Remove(instance.Yield());
        }

        public void Unsubscribe(IEnumerable<object> instances)
        {
            this.Remove(instances);
        }
    }
}
