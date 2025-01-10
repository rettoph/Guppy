using Guppy.Core.Messaging.Common;

namespace Guppy.Core.Messaging
{
    public class BusConfiguration
    {
        private const int _defaultPriority = 0;

        private readonly List<(Type type, int queue, int priority)> _queues = [];

        public void SetQueue<T>(int queue, int priority = _defaultPriority)
            where T : IMessage => this.SetQueue(typeof(T), queue, priority);

        public void SetQueue(Type type, int queue, int priority = _defaultPriority) => this._queues.Add((type, queue, priority));

        public (Type type, int queue)[] GetQueueConfigurations() => this._queues.OrderByDescending(x => x.priority).Select(x => (x.type, x.queue)).ToArray();
    }
}