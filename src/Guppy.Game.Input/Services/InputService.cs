using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Common.Implementations;
using Guppy.Common.Providers;
using Guppy.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.Input.Services
{
    internal class InputService : Broker<IInput>, IInputService, IBulkSubscriptionProvider
    {
        void IBulkSubscriptionProvider.Subscribe(IEnumerable<object> instances)
        {
            foreach(IBaseSubscriber<IInput> subscriber in instances.OfType<IBaseSubscriber<IInput>>())
            {
                this.Subscribe(subscriber);
            }
        }

        void IBulkSubscriptionProvider.Unsubscribe(IEnumerable<object> instances)
        {
            foreach (IBaseSubscriber<IInput> subscriber in instances.OfType<IBaseSubscriber<IInput>>())
            {
                this.Unsubscribe(subscriber);
            }
        }
    }
}
