using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IBroker : IDisposable
    {
        IPublisher this[Type type] { get; }

        /// <summary>
        /// Subscribe a subscriber with the broker
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subscriber"></param>
        public void Subscribe<T>(ISubscriber<T> subscriber) where T : IMessage;

        /// <summary>
        /// Unsubscribe a subscriber from the queue.
        /// </summary>
        /// <param name="subscriber"></param>
        public void Unsubscribe<T>(ISubscriber<T> subscriber) where T : IMessage;

        /// <summary>
        /// Publish an incoming message.
        /// </summary>
        /// <param name="message"></param>
        public void Publish(in IMessage message);

        void Subscribe(ISubscriber subscriber);

        void Unsubscribe(ISubscriber subscriber);
    }
}
