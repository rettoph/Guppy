using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IBroker<TBase> : IDisposable
        where TBase : notnull, IMessage
    {
        IPublisher<TBase> this[Type type] { get; }

        /// <summary>
        /// Subscribe a subscriber with the broker
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subscriber"></param>
        public void Subscribe<T>(IBaseSubscriber<TBase, T> subscriber) where T : TBase;

        /// <summary>
        /// Unsubscribe a subscriber from the queue.
        /// </summary>
        /// <param name="subscriber"></param>
        public void Unsubscribe<T>(IBaseSubscriber<TBase, T> subscriber) where T : TBase;

        /// <summary>
        /// Publish an incoming message.
        /// </summary>
        /// <param name="message"></param>
        public void Publish(in TBase message);

        void Subscribe(IBaseSubscriber<TBase> subscriber);

        void Unsubscribe(IBaseSubscriber<TBase> subscriber);
    }
}
