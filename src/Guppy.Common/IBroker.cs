using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IBroker<TBase> : IEnumerable<IPublisher<TBase>>
        where TBase : notnull
    {
        IPublisher<TBase> this[Type type] { get; }

        /// <summary>
        /// Subscribe a subscriber with the broker
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subscriber"></param>
        public void Subscribe<T>(ISubscriber<T> subscriber)
            where T : TBase;

        /// <summary>
        /// Unsubscribe a subscriber from the queue.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="processor"></param>
        public void Unsubscribe<T>(ISubscriber<T> processor)
            where T : TBase;

        /// <summary>
        /// Publish an incoming message.
        /// </summary>
        /// <param name="message"></param>
        public void Publish(Type type, in TBase message);

        /// <summary>
        /// Publish an incoming message.
        /// </summary>
        /// <param name="message"></param>
        public void Publish<T>(in T message)
            where T : TBase;
    }
}
