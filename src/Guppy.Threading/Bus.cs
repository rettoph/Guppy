using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading
{
    public partial class Bus : Broker
    {
        public const int DefaultPriority = 0;

        private Bus.Queue[] _queues = Array.Empty<Bus.Queue>();
        private Dictionary<Type, Bus.Queue> _typeQueues;

        public Bus(Dictionary<int, Type[]> queues) : base()
        {
            _typeQueues = new Dictionary<Type, Bus.Queue>();
            _queues = new Queue[queues.Count];
            int index = 0;

            foreach ((int priority, Type[] types) in queues)
            {
                Bus.Queue queue = _queues[index++] = new Queue(this, priority);

                foreach (Type type in types)
                {
                    _typeQueues.Add(type, queue);
                }
            }
        }

        public void Enqueue(object message)
        {
            if (_typeQueues.TryGetValue(message.GetType(), out Bus.Queue? queue))
            {
                queue.Enqueue(message);
                return;
            }
        }

        public void PublishEnqueued()
        {
            foreach (Bus.Queue queue in _queues)
            {
                queue.Publish();
            }
        }
    }
}
