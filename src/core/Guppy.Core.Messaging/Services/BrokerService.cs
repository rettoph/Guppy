﻿using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Extensions;
using Guppy.Core.Messaging.Common.Services;

namespace Guppy.Core.Messaging.Services
{
    internal sealed class BrokerService : IBrokerService, IDisposable
    {
        private readonly ILifetimeScope _scope;
        private readonly HashSet<IBaseBroker> _brokers;
        private readonly HashSet<IBaseSubscriber> _subscribers;

        public BrokerService(ILifetimeScope scope, IFiltered<IBaseBroker> brokers)
        {
            _scope = scope;

            _brokers = new HashSet<IBaseBroker>(brokers);
            _subscribers = new HashSet<IBaseSubscriber>();
        }

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
        }

        public void RemoveSubscribers<TSubscribers>() where TSubscribers : class
        {
            IFiltered<TSubscribers> instances = _scope.Resolve<IFiltered<TSubscribers>>();
            IEnumerable<IBaseSubscriber> subscribers = instances.OfType<IBaseSubscriber>();
            this.RemoveSubscribers(subscribers);
        }

        public IEnumerable<IBaseBroker> GetAll()
        {
            return _brokers;
        }
    }
}
