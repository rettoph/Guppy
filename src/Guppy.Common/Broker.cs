using System.Collections;

namespace Guppy.Common
{
    public class Broker<T> : IBroker<T>
        where T : notnull
    {
        private Dictionary<Type, IPublisher<T>> _publishers;

        public IPublisher<T> this[Type type] => _publishers[type];

        public Broker()
        {
            _publishers = new Dictionary<Type, IPublisher<T>>();
        }

        public void Subscribe<TMessage>(ISubscriber<TMessage> subscriber) 
            where TMessage : T
        {
            if (_publishers.TryGetValue(typeof(TMessage), out IPublisher<T>? publisher) && publisher is IPublisher<TMessage, T> casted)
            {
                casted.Subscribe(subscriber);
                return;
            }

            _publishers.Add(typeof(TMessage), new Publisher<TMessage, T>(subscriber));
        }

        public void Unsubscribe<TMessage>(ISubscriber<TMessage> processor) 
            where TMessage : T
        {
            if (_publishers.TryGetValue(typeof(TMessage), out IPublisher<T>? publisher) && publisher is IPublisher<TMessage, T> casted)
            {
                casted.Unsubscribe(processor);
            }
        }

        public void Publish(Type type, in T message)
        {
            if (_publishers.TryGetValue(type, out IPublisher<T>? publisher))
            {
                publisher.Publish(in message);
            }
        }

        public void Publish<TMessage>(in TMessage message)
            where TMessage : T
        {
            if (_publishers.TryGetValue(typeof(TMessage), out IPublisher<T>? publisher) && publisher is IPublisher<TMessage, T> casted)
            {
                casted.Publish(in message);
            }
        }

        public IEnumerator<IPublisher<T>> GetEnumerator()
        {
            return _publishers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
