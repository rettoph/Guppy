using Guppy.Common.Extensions;

namespace Guppy.Common.Implementations
{
    public class Broker<TBase> : IBroker<TBase>
        where TBase : IMessage
    {
        private readonly BrokerConfiguration<TBase> _configuration;
        private readonly IDictionary<Type, IPublisher<TBase>> _publishers;
        private readonly IDictionary<Type, Type[]> _aliases;
        private readonly Dictionary<IBaseSubscriber<TBase>, Subscription<TBase>[]> _subscriptions;

        public IPublisher<TBase> this[Type type] => _publishers[type];

        public Broker(IConfiguration<BrokerConfiguration<TBase>> configuration) : this(configuration.Value)
        {

        }
        protected Broker(BrokerConfiguration<TBase> configuration)
        {
            _configuration = configuration;
            _publishers = new Dictionary<Type, IPublisher<TBase>>();
            _aliases = new Dictionary<Type, Type[]>();
            _subscriptions = new Dictionary<IBaseSubscriber<TBase>, Subscription<TBase>[]>();
        }

        public void Subscribe<T>(IBaseSubscriber<TBase, T> subscriber)
            where T : TBase
        {
            if (_publishers.TryGetValue(typeof(T), out IPublisher<TBase>? publisher) && publisher is IPublisher<TBase, T> casted)
            {
                casted.Subscribe(subscriber);
                return;
            }

            _publishers.Add(typeof(T), new Publisher<TBase, T>(subscriber));
        }

        public void Unsubscribe<T>(IBaseSubscriber<TBase, T> processor)
            where T : TBase
        {
            if (_publishers.TryGetValue(typeof(T), out IPublisher<TBase>? publisher) && publisher is IPublisher<TBase, T> casted)
            {
                casted.Unsubscribe(processor);
            }
        }

        public void Subscribe(IBaseSubscriber<TBase> subscriber)
        {
            if (_subscriptions.ContainsKey(subscriber))
            {
                return;
            }

            Subscription<TBase>[] subscriptions = subscriber.GetSubscriptions().ToArray();
            _subscriptions.Add(subscriber, subscriptions);

            foreach (Subscription<TBase> subscription in subscriptions)
            {
                subscription.Subscribe(this);
            }
        }

        public void Unsubscribe(IBaseSubscriber<TBase> subscriber)
        {
            if (!_subscriptions.Remove(subscriber, out Subscription<TBase>[]? subscriptions))
            {
                return;
            }

            foreach (Subscription<TBase> subscription in subscriptions)
            {
                subscription.Unsubscribe(this);
            }
        }

        public void Publish(in TBase message)
        {
            if(!_aliases.TryGetValue(message.Type, out Type[]? aliases))
            {
                aliases = _configuration.GetAliases(message);
                _aliases.Add(message.Type, aliases);
            }

            foreach(Type alias in aliases)
            {
                if (_publishers.TryGetValue(alias, out IPublisher<TBase>? publisher))
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

            foreach (IPublisher<TBase> publisher in _publishers.Values)
            {
                publisher.Dispose();
            }
        }
    }
}
