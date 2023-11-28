using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Common.Implementations;
using Guppy.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Services
{
    internal class InputService : Broker<IInput>, IInputService, IBulkSubscriptionProvider
    {
        public InputService(IConfiguration<BrokerConfiguration<IInput>> configuration) : base(configuration)
        {
        }

        void IBulkSubscriptionProvider.Subscribe(IEnumerable<object> instances)
        {
            this.SubscribeMany(instances.OfType<IBaseSubscriber<IInput>>());
        }

        void IBulkSubscriptionProvider.Unsubscribe(IEnumerable<object> instances)
        {
            this.UnsubscribeMany(instances.OfType<IBaseSubscriber<IInput>>());
        }
    }
}
