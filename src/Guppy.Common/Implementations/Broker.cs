using Guppy.Common.Extensions;
using Microsoft.Extensions.Options;
using Serilog;
using System.Collections;

namespace Guppy.Common.Implementations
{
    public class Broker : IBroker
    {
        private readonly BrokerConfiguration _configuration;
        private readonly IDictionary<Type, IPublisher> _publishers;
        private readonly IDictionary<Type, Type[]> _aliases;
        private readonly Dictionary<ISubscriber, Subscription[]> _subscriptions;

        public IPublisher this[Type type] => _publishers[type];

        public Broker(IOptions<BrokerConfiguration> configuration) : this(configuration.Value)
        {

        }
        protected Broker(BrokerConfiguration configuration)
        {
            _configuration = configuration;
            _publishers = new Dictionary<Type, IPublisher>();
            _aliases = new Dictionary<Type, Type[]>();
            _subscriptions = new Dictionary<ISubscriber, Subscription[]>();
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

        public void Subscribe(ISubscriber subscriber)
        {
            if (_subscriptions.ContainsKey(subscriber))
            {
                return;
            }

            Subscription[] subscriptions = subscriber.GetSubscriptions().ToArray();
            _subscriptions.Add(subscriber, subscriptions);

            foreach (Subscription subscription in subscriptions)
            {
                subscription.Subscribe(this);
            }
        }

        public void Unsubscribe(ISubscriber subscriber)
        {
            if (!_subscriptions.Remove(subscriber, out Subscription[]? subscriptions))
            {
                return;
            }

            foreach (Subscription subscription in subscriptions)
            {
                subscription.Unsubscribe(this);
            }
        }

        public void Publish(in IMessage message)
        {
            if(!_aliases.TryGetValue(message.Type, out Type[]? aliases))
            {
                aliases = _configuration.GetAliases(message);
                _aliases.Add(message.Type, aliases);
            }

            foreach(Type alias in aliases)
            {
                if (_publishers.TryGetValue(alias, out IPublisher? publisher))
                {
                    if(message is IDisposable disposable)
                    {
                        using (disposable)
                        {
                            publisher.Publish(message);
                            return;
                        }
                    }

                    publisher.Publish(in message);
                }
            }
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);

            foreach (IPublisher publisher in _publishers.Values)
            {
                publisher.Dispose();
            }
        }
    }
}
