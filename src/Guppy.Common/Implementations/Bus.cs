﻿using Guppy.Common.Extensions;
using Guppy.Common.Utilities;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    internal class Bus : IBus, IBroker, IDisposable
    {
        private const int DefaultQueue = 0;

        private readonly IBroker _broker;
        private readonly IBusQueue[] _queues;
        private readonly Dictionary<Type, IBusQueue> _typeMap;
        private readonly IBusQueue _default;
        private readonly Dictionary<ISubscriber, Subscription[]> _subscriptions;

        public IPublisher this[Type type] => _broker[type];

        public Guid Id { get; } = Guid.NewGuid();

        public Bus(
            IBroker broker,
            IOptions<BusConfiguration> configuration)
        {
            _broker = broker;
            _subscriptions = new Dictionary<ISubscriber, Subscription[]>();

            _queues = configuration.Value.TypeQueues.Select(x => x.Queue).Concat(DefaultQueue.Yield())
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new BusQueue(x, _broker))
                .ToArray();

            _default = this.GetQueue(DefaultQueue);

            _typeMap = configuration.Value.TypeQueues.ToDictionary(
                keySelector: x => x.Type,
                elementSelector: x => this.GetQueue(x.Queue));
        }

        public void Dispose()
        {
            _broker.Dispose();
        }

        public void Flush()
        {
            foreach(var queue in _queues)
            {
                queue.Flush();
            }
        }

        public void Enqueue(in IMessage message)
        {
            this.GetQueue(message.Type).Enqueue(message);
        }

        public void Publish(in IMessage message)
        {
            _broker.Publish(message);
        }

        public void Subscribe<T>(ISubscriber<T> subscriber)
            where T : notnull, IMessage
        {
            _broker.Subscribe(subscriber);
        }

        public void Unsubscribe<T>(ISubscriber<T> processor)

            where T : notnull, IMessage
        {
            _broker.Unsubscribe(processor);
        }

        private IBusQueue GetQueue(Type type)
        {
            if(_typeMap.TryGetValue(type, out var queue))
            {
                return queue;
            }

            return _default;
        }

        private IBusQueue GetQueue(int id)
        {
            return _queues.FirstOrDefault(x => x.Id == id) ?? _default;
        }

        public void Subscribe(ISubscriber subscriber)
        {
            if(_subscriptions.ContainsKey(subscriber))
            {
                return;
            }

            Subscription[] subscriptions = subscriber.GetSubscriptions().ToArray();
            _subscriptions.Add(subscriber, subscriptions);

            foreach(Subscription subscription in subscriptions)
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
    }
}
