using Guppy.Core.Common;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Implementations;

namespace Guppy.Core.Messaging
{
    internal class Bus : Broker<IMessage>, IBus
    {
        private readonly (Type type, int queue)[] _queueConfigurations;
        private readonly Dictionary<Type, Queue<IMessage>> _typeQueues;
        private readonly Dictionary<int, Queue<IMessage>> _queuesDict;
        private readonly Queue<IMessage>[] _queuesArray;

        public Bus(IConfiguration<BusConfiguration> configuration)
        {
            this._typeQueues = [];
            this._queueConfigurations = configuration.Value.GetQueueConfigurations();

            this._queuesDict = this._queueConfigurations.Select(x => x.queue)
                .Concat(0.Yield())
                .Distinct()
                .ToDictionary(x => x, x => new Queue<IMessage>());

            this._queuesArray = this._queuesDict.OrderBy(x => x.Key)
                .Select(x => x.Value)
                .ToArray();
        }

        public void Enqueue(in IMessage message) => this.GetTypeQueue(message.Type).Enqueue(message);

        public void Flush()
        {
            foreach (Queue<IMessage> queue in this._queuesArray)
            {
                while (queue.TryDequeue(out IMessage? message))
                {
                    this.Publish(message);
                }
            }
        }

        private Queue<IMessage> GetTypeQueue(Type type)
        {
            if (this._typeQueues.TryGetValue(type, out Queue<IMessage>? queue))
            {
                return queue;
            }

            int queueId = this._queueConfigurations.FirstOrDefault(x => x.type.IsAssignableFrom(type)).queue;
            queue = this._queuesDict[queueId];
            this._typeQueues.Add(type, queue);

            return queue;
        }
    }
}