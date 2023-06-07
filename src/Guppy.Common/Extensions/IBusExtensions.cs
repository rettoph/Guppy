using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Extensions
{
    public static class IBusExtensions
    {
        public static void SubscribeMany(this IBus bus, IEnumerable<ISubscriber> subscribers)
        {
            foreach(ISubscriber subscriber in subscribers)
            {
                bus.Subscribe(subscriber);
            }
        }

        public static void UnsubscribeMany(this IBus bus, IEnumerable<ISubscriber> subscribers)
        {
            foreach (ISubscriber subscriber in subscribers)
            {
                bus.Unsubscribe(subscriber);
            }
        }
    }
}
