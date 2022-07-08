using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public class Broker<TMessage> : IBroker<TMessage>
        where TMessage : notnull
    {
        private Dictionary<Type, IPublisher<TMessage>> _publishers;

        public IPublisher<TMessage> this[Type type] => _publishers[type];

        public Broker()
        {
            _publishers = new Dictionary<Type, IPublisher<TMessage>>();
        }

        public void Subscribe<T>(ISubscriber<T> subscriber) 
            where T : TMessage
        {
            if (_publishers.TryGetValue(typeof(T), out IPublisher<TMessage>? publishers) && publishers is IPublisher<T, TMessage> casted)
            {
                casted.Subscribe(subscriber);
                return;
            }

            _publishers.Add(typeof(T), new Publisher<T, TMessage>(subscriber));
        }

        public void Unsubscribe<T>(ISubscriber<T> processor) 
            where T : TMessage
        {
            if (_publishers.TryGetValue(typeof(T), out IPublisher<TMessage>? publishers) && publishers is IPublisher<T, TMessage> casted)
            {
                casted.Unsubscribe(processor);
            }
        }

        public void Publish(in TMessage message)
        {
            if (_publishers.TryGetValue(message.GetType(), out IPublisher<TMessage>? publishers))
            {
                publishers.Publish(in message);
            }
        }

        public void Publish<T>(in T message)
            where T : TMessage
        {
            if (_publishers.TryGetValue(typeof(T), out IPublisher<TMessage>? publishers) && publishers is IPublisher<T, TMessage> casted)
            {
                casted.Publish(in message);
            }
        }

        public IEnumerator<IPublisher<TMessage>> GetEnumerator()
        {
            return _publishers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
