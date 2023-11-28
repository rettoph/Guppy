using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public record Subscription<TBase>(IBaseSubscriber<TBase> Subscriber, Type Type, Action<IBroker<TBase>> Subscribe, Action<IBroker<TBase>> Unsubscribe)
        where TBase : IMessage
    {
    }
}
