using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    [GuppyFilter<IGuppy>]
    public class BulkGuppyBrokerSubscriptionProvider<TBroker, TMessage> : IBulkSubscriptionProvider
        where TMessage : IMessage
        where TBroker : IBroker<TMessage>
    {
        private readonly Lazy<TBroker> _service;

        public BulkGuppyBrokerSubscriptionProvider(Lazy<TBroker> bus)
        {
            _service = bus;
        }

        public void Subscribe(IEnumerable<object> instances)
        {
            _service.Value.SubscribeMany(instances.OfType<IBaseSubscriber<TMessage>>());
        }

        public void Unsubscribe(IEnumerable<object> instances)
        {
            _service.Value.UnsubscribeMany(instances.OfType<IBaseSubscriber<TMessage>>());
        }
    }
}
