using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Messaging;

namespace Guppy.Providers
{
    [GuppyFilter<IGuppy>]
    public class BulkGuppyBrokerSubscriptionProvider<TBroker, TMessage> : IBulkSubscriptionProvider
        where TMessage : class, IMessage
        where TBroker : IBroker<TMessage>
    {
        private readonly Lazy<TBroker> _service;

        public BulkGuppyBrokerSubscriptionProvider(Lazy<TBroker> bus)
        {
            _service = bus;
        }

        public void Subscribe(IEnumerable<object> instances)
        {
            foreach (IBaseSubscriber<TMessage> subscriber in instances.OfType<IBaseSubscriber<TMessage>>())
            {
                _service.Value.Subscribe(subscriber);
            }
        }

        public void Unsubscribe(IEnumerable<object> instances)
        {
            foreach (IBaseSubscriber<TMessage> subscriber in instances.OfType<IBaseSubscriber<TMessage>>())
            {
                _service.Value.Unsubscribe(subscriber);
            }
        }
    }
}
