namespace Guppy.Common.Implementations
{
    internal class Bus : IBus, IBroker<IMessage>, IDisposable
    {
        private const int DefaultQueue = 0;

        private readonly IBroker<IMessage> _broker;
        private readonly IBusQueue<IMessage>[] _queues;
        private readonly Dictionary<Type, IBusQueue<IMessage>> _typeMap;
        private readonly IBusQueue<IMessage> _default;

        public IPublisher<IMessage> this[Type type] => _broker[type];

        public Guid Id { get; } = Guid.NewGuid();

        public Bus(
            IBroker<IMessage> broker,
            IConfiguration<BusConfiguration> configuration)
        {
            _broker = broker;

            _queues = configuration.Value.TypeQueues.Select(x => x.Queue).Concat(DefaultQueue.Yield())
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new BusQueue<IMessage>(x, _broker))
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

        public void Subscribe<T>(IBaseSubscriber<IMessage, T> subscriber)
            where T : IMessage
        {
            _broker.Subscribe(subscriber);
        }

        public void Unsubscribe<T>(IBaseSubscriber<IMessage, T> subscriber)
            where T : IMessage
        {
            _broker.Unsubscribe(subscriber);
        }

        public void Subscribe(IBaseSubscriber<IMessage> subscriber)
        {
            _broker.Subscribe(subscriber);
        }

        public void Unsubscribe(IBaseSubscriber<IMessage> subscriber)
        {
            _broker.Unsubscribe(subscriber);
        }

        private IBusQueue<IMessage> GetQueue(Type type)
        {
            if(_typeMap.TryGetValue(type, out var queue))
            {
                return queue;
            }

            return _default;
        }

        private IBusQueue<IMessage> GetQueue(int id)
        {
            return _queues.FirstOrDefault(x => x.Id == id) ?? _default;
        }
    }
}
