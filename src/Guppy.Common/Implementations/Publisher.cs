using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    internal abstract class Publisher : IPublisher
    {
        public Type Type { get; }

        public Publisher(Type type)
        {
            Type = type;
        }

        public abstract void Publish(in IMessage message);
        public abstract void Dispose();
    }

    internal class Publisher<T> : Publisher, IPublisher<T>
        where T : notnull, IMessage
    {
        private delegate void ProcessDelegate(in T message);

        private ProcessDelegate? _subscribers;

        public Publisher(ISubscriber<T> subscriber) : base(typeof(T))
        {
            _subscribers = subscriber.Process;
        }

        public override void Publish(in IMessage message)
        {
            if (message is T casted)
            {
                this.Publish(casted);
                return;
            }

            throw new ArgumentException($"{nameof(Publisher)}::{nameof(Publish)} - Unable to assign {typeof(T).Name} from {message.GetType().Name}", nameof(message));
        }

        public void Publish(in T message)
        {
            _subscribers?.Invoke(in message);
        }

        public void Subscribe(ISubscriber<T> subscriber)
        {
            _subscribers += subscriber.Process;
        }

        public void Unsubscribe(ISubscriber<T> subscriber)
        {
            _subscribers -= subscriber.Process;
        }

        public override void Dispose()
        {
            _subscribers = null;
        }
    }
}
