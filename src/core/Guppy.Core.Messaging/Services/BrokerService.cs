using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Extensions;
using Guppy.Core.Messaging.Common.Services;

namespace Guppy.Core.Messaging.Services
{
    internal sealed class BrokerService(ILifetimeScope scope, IFiltered<IBaseBroker> brokers) : IBrokerService, IDisposable
    {
        private readonly ILifetimeScope _scope = scope;
        private readonly HashSet<IBaseBroker> _brokers = new HashSet<IBaseBroker>(brokers);
        private readonly HashSet<IBaseSubscriber> _subscribers = new HashSet<IBaseSubscriber>();
        private readonly Dictionary<Type, IBaseSubscriber[]> _scopeSubscriptions = new Dictionary<Type, IBaseSubscriber[]>();

        public void Dispose()
        {
            foreach (IBaseBroker broker in _brokers)
            {
                broker.TryUnsubscribeMany(_subscribers);
            }

            _brokers.Clear();
        }

        public void Add(IBaseBroker broker)
        {
            if (_brokers.Add(broker) == false)
            {
                return;
            }

            broker.TrySubscribeMany(_subscribers);
        }

        public void Remove(IBaseBroker broker)
        {
            if (_brokers.Remove(broker) == false)
            {
                return;
            }

            broker.TryUnsubscribeMany(_subscribers);
        }

        public void AddSubscribers(IEnumerable<IBaseSubscriber> subscribers)
        {
            foreach (IBaseSubscriber subscriber in subscribers)
            {
                if (_subscribers.Add(subscriber) == false)
                {
                    continue;
                }

                foreach (IBaseBroker broker in _brokers)
                {
                    broker.TrySubscribe(subscriber);
                }
            }
        }

        public void RemoveSubscribers(IEnumerable<IBaseSubscriber> subscribers)
        {
            foreach (IBaseSubscriber subscriber in subscribers)
            {
                if (_subscribers.Remove(subscriber) == false)
                {
                    continue;
                }

                foreach (IBaseBroker broker in _brokers)
                {
                    broker.TryUnsubscribe(subscriber);
                }
            }
        }

        public void AddSubscribers<TSubscribers>() where TSubscribers : class
        {
            IFiltered<TSubscribers> instances = _scope.Resolve<IFiltered<TSubscribers>>();
            IEnumerable<IBaseSubscriber> subscribers = instances.OfType<IBaseSubscriber>();
            this.AddSubscribers(subscribers);
            _scopeSubscriptions.Add(typeof(TSubscribers), subscribers.ToArray());
        }

        public void RemoveSubscribers<TSubscribers>() where TSubscribers : class
        {
            if (_scopeSubscriptions.TryGetValue(typeof(TSubscribers), out IBaseSubscriber[]? subscribers) == false)
            {
                return;
            }

            this.RemoveSubscribers(subscribers);
        }

        public IEnumerable<IBaseBroker> GetAll()
        {
            return _brokers;
        }
    }
}
