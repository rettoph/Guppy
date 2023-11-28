using Guppy.Common;
using Guppy.Common.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    /// <summary>
    /// TODO: Refactor this, TypeMessage struct and
    /// IDisposable check? 
    /// 
    /// The IDisposable check exists soley to recycle
    /// INetMessage instances passed in here, but idk if
    /// thats even beneficial. More testing needed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class BusQueue<TBase> : IBusQueue<TBase>
        where TBase : IMessage
    {
        private readonly IBroker<TBase> _broker;
        private ConcurrentQueue<TBase> _queue;

        public int Id { get; }

        public BusQueue(int id, IBroker<TBase> broker)
        {
            this.Id = id;

            _queue = new ConcurrentQueue<TBase>();
            _broker = broker;
        }

        public void Enqueue(in TBase message)
        {
            _queue.Enqueue(message);
        }

        public void Flush()
        {
            int errors = 0;
            
            while(!_queue.IsEmpty && errors != 5)
            {
                if (_queue.TryDequeue(out var message))
                {
                    _broker.Publish(message);
                    continue;
                }

                errors++;
            }
        }
    }
}
