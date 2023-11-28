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
        private static class MethodInfo<TBase>
            where TBase : IMessage
        {
            public static readonly MethodInfo Subscribe = typeof(IBroker<TBase>).GetMethod(nameof(IBroker<TBase>.Subscribe), 1, new[] { typeof(IBaseSubscriber<,>).MakeGenericType(typeof(TBase), Type.MakeGenericMethodParameter(0)) }) ?? throw new NotImplementedException();
            public static readonly MethodInfo Unsubscribe = typeof(IBroker<TBase>).GetMethod(nameof(IBroker<TBase>.Unsubscribe), 1, new[] { typeof(IBaseSubscriber<,>).MakeGenericType(typeof(TBase), Type.MakeGenericMethodParameter(0)) }) ?? throw new NotImplementedException();
        }


        public static IEnumerable<Subscription<TBase>> GetSubscriptions<TBase>(this IBaseSubscriber<TBase> subscriber)
            where TBase : IMessage
        {
            foreach (var interfaceType in subscriber.GetType().GetConstructedGenericTypes(typeof(IBaseSubscriber<,>)))
            {
                var messageType = interfaceType.GetGenericArguments()[1];

                yield return new Subscription<TBase>(
                    Subscriber: subscriber,
                    Type: interfaceType,
                    Subscribe: BrokerMethod<TBase>(subscriber, messageType, MethodInfo<TBase>.Subscribe),
                    Unsubscribe: BrokerMethod<TBase>(subscriber, messageType, MethodInfo<TBase>.Unsubscribe)
                );
            }
        }

        private static Action<IBroker<TBase>> BrokerMethod<TBase>(IBaseSubscriber<TBase> subscriber, Type messageType, MethodInfo methodInfo)
            where TBase : IMessage
        {
            var method = methodInfo.MakeGenericMethod(messageType);

            void Method(IBroker<TBase> broker)
            {
                method.Invoke(broker, new[] { subscriber });
            }

            return Method;
        }
    }
}
