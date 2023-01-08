﻿using Guppy.Common;
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
    internal sealed class BusQueue : IBusQueue
    {
        private readonly IBroker _broker;
        private ConcurrentQueue<IMessage> _queue;

        public int Id { get; }

        public BusQueue(int id, IBroker broker)
        {
            this.Id = id;

            _queue = new ConcurrentQueue<IMessage>();
            _broker = broker;
        }

        public void Enqueue(in IMessage message)
        {
            _queue.Enqueue(message);
        }

        public void Publish(in IMessage message)
        {
            if (message is IDisposable disposable)
            {
                using (disposable)
                {
                    _broker.Publish(in message);
                    return;
                }
            }

            _broker.Publish(message);
        }

        public void Flush()
        {
            int errors = 0;
            
            while(!_queue.IsEmpty && errors != 5)
            {
                if (_queue.TryDequeue(out var message))
                {
                    this.Publish(message);
                    continue;
                }

                errors++;
            }
        }
    }
}
