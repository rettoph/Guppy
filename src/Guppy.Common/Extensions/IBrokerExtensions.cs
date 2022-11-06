using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Extensions
{
    public static class IBrokerExtensions
    {
        private static readonly MethodInfo SubscribeMethodInfo = typeof(IBroker).GetMethod(nameof(IBroker.Subscribe)) ?? throw new NotImplementedException();
        private static readonly MethodInfo UnsubscribeMethodInfo = typeof(IBroker).GetMethod(nameof(IBroker.Unsubscribe)) ?? throw new NotImplementedException();

        /// <summary>
        /// Using reflection, subscribe all <see cref="ISubscriber{T}"/>
        /// implementations to the current <see cref="IBroker"/>.
        /// </summary>
        /// <param name="subscriber"></param>
        public static void SubscribeAll(this IBroker broker, ISubscriber subscriber)
        {
            broker.InvokeAll(subscriber, SubscribeMethodInfo);
        }

        /// <summary>
        /// Using reflection, subscribe all <see cref="ISubscriber{T}"/>
        /// implementations to the current <see cref="IBroker"/>.
        /// </summary>
        /// <param name="subscriber"></param>
        public static void UnsubscribeAll(this IBroker broker, ISubscriber subscriber)
        {
            broker.InvokeAll(subscriber, UnsubscribeMethodInfo);
        }

        private static void InvokeAll(this IBroker broker, ISubscriber subscriber, MethodInfo methodInfo)
        {
            var interfaceTypes = subscriber.GetType().GetInterfaces();

            foreach (var interfaceType in interfaceTypes)
            {
                if (interfaceType.IsConstructedGenericType && interfaceType.GetGenericTypeDefinition() == typeof(ISubscriber<>))
                {
                    var messageType = interfaceType.GetGenericArguments()[0];

                    var method = methodInfo.MakeGenericMethod(messageType);

                    method.Invoke(broker, new[] { subscriber });
                }
            }
        }
    }
}
