using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public record Subscription(ISubscriber Subscriber, Type Type, Action<IBroker> Subscribe, Action<IBroker> Unsubscribe)
    {
        public int Order => this.Subscriber.GetOrderAs(this.Type);
    }
}
