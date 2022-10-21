using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IBroker<T> : IEnumerable<IPublisher<T>>
        where T : notnull, IMessage
    {
        IPublisher<T> this[Type type] { get; }

        /// <summary>
        /// Subscribe a subscriber with the broker
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subscriber"></param>
        public void Subscribe<TMessage>(ISubscriber<TMessage> subscriber)
            where TMessage : T;

        /// <summary>
        /// Unsubscribe a subscriber from the queue.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="processor"></param>
        public void Unsubscribe<TMessage>(ISubscriber<TMessage> processor)
            where TMessage : T;

        /// <summary>
        /// Publish an incoming message.
        /// </summary>
        /// <param name="message"></param>
        public void Publish(in T message);
    }
}
