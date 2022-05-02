using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading
{
    public interface IBroker
    {
        /// <summary>
        /// Subscribe a subscriber with the broker
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subscriber"></param>
        public void Subscribe<T>(ISubscriber<T> subscriber);

        /// <summary>
        /// Unsubscribe a subscriber from the queue.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="processor"></param>
        public void Unsubscribe<T>(ISubscriber<T> processor);

        /// <summary>
        /// Publish an incoming message.
        /// </summary>
        /// <param name="message"></param>
        public bool Publish(in object message);

        /// <summary>
        /// Publish an incoming message.
        /// </summary>
        /// <param name="message"></param>
        public bool Publish<T>(in T message);
    }
}
