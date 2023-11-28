using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Extensions
{
    public static class IBrokerExtensions
    {
        public static void SubscribeMany<TBase>(this IBroker<TBase> broker, IEnumerable<IBaseSubscriber<TBase>> subscribers)
            where TBase : IMessage
        {
            foreach(IBaseSubscriber<TBase> subscriber in subscribers)
            {
                broker.Subscribe(subscriber);
            }
        }

        public static void UnsubscribeMany<TBase>(this IBroker<TBase> broker, IEnumerable<IBaseSubscriber<TBase>> subscribers)
            where TBase : IMessage
        {
            foreach (IBaseSubscriber<TBase> subscriber in subscribers)
            {
                broker.Unsubscribe(subscriber);
            }
        }
    }
}
