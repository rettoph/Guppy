using System.Collections;

namespace Guppy.Common.Implementations
{
    public class Broker : IBroker
    {
        private Dictionary<Type, IPublisher> _publishers;

        public IPublisher this[Type type] => _publishers[type];

        public Broker()
        {
            _publishers = new Dictionary<Type, IPublisher>();
        }

        public void Subscribe<T>(ISubscriber<T> subscriber)
            where T : notnull, IMessage
        {
            if (_publishers.TryGetValue(typeof(T), out IPublisher? publisher) && publisher is IPublisher<T> casted)
            {
                casted.Subscribe(subscriber);
                return;
            }

            _publishers.Add(typeof(T), new Publisher<T>(subscriber));
        }

        public void Unsubscribe<T>(ISubscriber<T> processor)
            where T : notnull, IMessage
        {
            if (_publishers.TryGetValue(typeof(T), out IPublisher? publisher) && publisher is IPublisher<T> casted)
            {
                casted.Unsubscribe(processor);
            }
        }

        public void Publish(in IMessage message)
        {
            if (_publishers.TryGetValue(message.PublishType, out IPublisher? publisher))
            {
                publisher.Publish(in message);
            }
        }

        public void Dispose()
        {
            foreach (IPublisher publisher in _publishers.Values)
            {
                publisher.Dispose();
            }
        }
    }
}
