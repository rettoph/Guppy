using Guppy.Common.Extensions;
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
        private readonly IFiltered<ISubscriber> _subscribers;
        private List<Subscription> _subscriptions;

        public IPublisher this[Type type] => _broker[type];

        public Guid Id { get; } = Guid.NewGuid();

        public Bus(
            IBroker broker,
            IFiltered<ISubscriber> subscribers,
            IOptions<BusConfiguration> configuration)
        {
            _subscribers = subscribers;
            _broker = broker;
            _subscriptions = default!;

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

        public void Initialize()
        {
            _subscriptions = _subscribers.Instances
                .Select(x => x.GetSubscriptions())
                .SelectMany(x => x)
                .OrderBy(x => x.Order)
                .ToList();

            foreach (Subscription subscription in _subscriptions)
            {
                subscription.Subscribe(this);
            }
        }

        public void Dispose()
        {
            foreach (Subscription subscription in _subscriptions)
            {
                subscription.Unsubscribe(this);
            }

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
    }
}
