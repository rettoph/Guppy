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
    internal class BulkBusSubscriptionProvider : IBulkSubscriptionProvider
    {
        private readonly Lazy<IBus> _bus;

        public BulkBusSubscriptionProvider(Lazy<IBus> bus)
        {
            _bus = bus;
        }

        public void Subscribe(IEnumerable<object> instances)
        {
            _bus.Value.SubscribeMany(instances.OfType<IBaseSubscriber<IMessage>>());
        }

        public void Unsubscribe(IEnumerable<object> instances)
        {
            _bus.Value.UnsubscribeMany(instances.OfType<IBaseSubscriber<IMessage>>());
        }
    }
}
