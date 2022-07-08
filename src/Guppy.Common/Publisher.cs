using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    internal abstract class Publisher<TMessage> : IPublisher<TMessage>
        where TMessage : notnull
    {
        public Type Type { get; }

        public Publisher(Type type)
        {
            this.Type = type;
        }

        public abstract void Publish(in TMessage message);
    }

    internal class Publisher<T, TMessage> : Publisher<TMessage>, IPublisher<T, TMessage>
        where TMessage : notnull
        where T : TMessage
    {
        private delegate void ProcessDelegate(in T message);

        private ProcessDelegate? _subscribers;

        public Publisher(ISubscriber<T> subscriber) : base(typeof(T))
        {
            _subscribers = subscriber.Process;
        }

        public override void Publish(in TMessage message)
        {
            if(message is T casted)
            {
                this.Publish(casted);
                return;
            }

            throw new ArgumentException(nameof(message));
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
    }
}
