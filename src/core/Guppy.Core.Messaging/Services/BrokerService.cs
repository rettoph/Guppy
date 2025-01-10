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
        private readonly HashSet<IBaseBroker> _brokers = new(brokers);
        private readonly HashSet<IBaseSubscriber> _subscribers = [];
        private readonly Dictionary<Type, IBaseSubscriber[]> _scopeSubscriptions = [];

        public void Dispose()
        {
            foreach (IBaseBroker broker in this._brokers)
            {
                broker.TryUnsubscribeMany(this._subscribers);
            }

            this._brokers.Clear();
        }

        public void Add(IBaseBroker broker)
        {
            if (this._brokers.Add(broker) == false)
            {
                return;
            }

            broker.TrySubscribeMany(this._subscribers);
        }

        public void Remove(IBaseBroker broker)
        {
            if (this._brokers.Remove(broker) == false)
            {
                return;
            }

            broker.TryUnsubscribeMany(this._subscribers);
        }

        public void AddSubscribers(IEnumerable<IBaseSubscriber> subscribers)
        {
            foreach (IBaseSubscriber subscriber in subscribers)
            {
                if (this._subscribers.Add(subscriber) == false)
                {
                    continue;
                }

                foreach (IBaseBroker broker in this._brokers)
                {
                    broker.TrySubscribe(subscriber);
                }
            }
        }

        public void RemoveSubscribers(IEnumerable<IBaseSubscriber> subscribers)
        {
            foreach (IBaseSubscriber subscriber in subscribers)
            {
                if (this._subscribers.Remove(subscriber) == false)
                {
                    continue;
                }

                foreach (IBaseBroker broker in this._brokers)
                {
                    broker.TryUnsubscribe(subscriber);
                }
            }
        }

        public void AddSubscribers<TSubscribers>() where TSubscribers : class
        {
            IFiltered<TSubscribers> instances = this._scope.Resolve<IFiltered<TSubscribers>>();
            IEnumerable<IBaseSubscriber> subscribers = instances.OfType<IBaseSubscriber>();
            this.AddSubscribers(subscribers);
            this._scopeSubscriptions.Add(typeof(TSubscribers), subscribers.ToArray());
        }

        public void RemoveSubscribers<TSubscribers>() where TSubscribers : class
        {
            if (this._scopeSubscriptions.TryGetValue(typeof(TSubscribers), out IBaseSubscriber[]? subscribers) == false)
            {
                return;
            }

            this.RemoveSubscribers(subscribers);
        }

        public IEnumerable<IBaseBroker> GetAll() => this._brokers;
    }
}