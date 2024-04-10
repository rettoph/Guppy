using Guppy.Attributes;
using Guppy.Messaging;

namespace Guppy.Providers
{
    [GuppyFilter<IGuppy>]
    public class BulkGuppyBrokerSubscriptionProvider<TBroker, TMessage> : IMagicBroker
        where TMessage : class, IMessage
        where TBroker : IBroker<TMessage>
    {
        private readonly Lazy<TBroker> _broker;

        public BulkGuppyBrokerSubscriptionProvider(Lazy<TBroker> broker)
        {
            _broker = broker;
        }

        public void Subscribe(IEnumerable<object> instances)
        {
            foreach (IBaseSubscriber<TMessage> subscriber in instances.OfType<IBaseSubscriber<TMessage>>())
            {
                _broker.Value.Subscribe(subscriber);
            }
        }

        public void Unsubscribe(IEnumerable<object> instances)
        {
            foreach (IBaseSubscriber<TMessage> subscriber in instances.OfType<IBaseSubscriber<TMessage>>())
            {
                _broker.Value.Unsubscribe(subscriber);
            }
        }
    }
}
