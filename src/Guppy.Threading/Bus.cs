using Guppy.Threading.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading
{
    public partial class Bus : Broker
    {
        private Bus.Queue[] _queues = Array.Empty<Bus.Queue>();
        private Dictionary<Type, Bus.Queue> _typeQueues;

        public Bus(IEnumerable<BusMessageDefinition> messages) : base()
        {
            var queueConfs = messages.OrderBy(x => x.Queue).GroupBy(x => x.Queue).ToList();

            _typeQueues = new Dictionary<Type, Bus.Queue>();
            _queues = new Queue[queueConfs.Count()];
            int index = 0;

            foreach (IGrouping<int, BusMessageDefinition> queueConf in queueConfs)
            {
                Bus.Queue queue = _queues[index++] = new Queue(this, queueConf.Key);

                foreach (BusMessageDefinition messageConf in queueConf)
                {
                    _typeQueues.Add(messageConf.Type, queue);
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

            throw new ArgumentException($"{nameof(Bus)}:{nameof(Enqueue)} - Unknown message type '{message.GetType().GetPrettyName()}' recieved. Please ensure a matching {nameof(BusMessageDefinition)} is defined.", nameof(message));
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
