using Guppy.Common.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Extensions
{
    public static class ISubscriberExtensions
    {
        private static readonly MethodInfo SubscribeMethodInfo = typeof(IBroker).GetMethod(nameof(IBroker.Subscribe)) ?? throw new NotImplementedException();
        private static readonly MethodInfo UnsubscribeMethodInfo = typeof(IBroker).GetMethod(nameof(IBroker.Unsubscribe)) ?? throw new NotImplementedException();

        public static IEnumerable<Subscription> GetSubscriptions(this ISubscriber subscriber)
        {
            foreach (var interfaceType in subscriber.GetType().GetConstructedGenericTypes(typeof(ISubscriber<>)))
            {
                var messageType = interfaceType.GetGenericArguments()[0];

                yield return new Subscription(
                    Subscriber: subscriber,
                    Type: interfaceType,
                    Subscribe: BrokerMethod(subscriber, messageType, SubscribeMethodInfo),
                    Unsubscribe: BrokerMethod(subscriber, messageType, UnsubscribeMethodInfo)
                );
            }
        }

        private static Action<IBroker> BrokerMethod(ISubscriber subscriber, Type messageType, MethodInfo methodInfo)
        {
            var method = methodInfo.MakeGenericMethod(messageType);

            void Method(IBroker broker)
            {
                method.Invoke(broker, new[] { subscriber });
            }

            return Method;
        }
    }
}
