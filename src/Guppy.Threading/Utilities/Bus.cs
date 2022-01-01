using Guppy.EntityComponent.DependencyInjection;
using Guppy.Threading.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Utilities
{
    public class Bus : Bus<IData>
    {
        #region Classes
        public class Queue
        {
            public readonly String Name;
            public readonly Int32 Order;

            public Queue(string name, int order)
            {
                this.Name = name;
                this.Order = order;
            }
        }
        #endregion
    }

    public class Bus<TData> : DataProcessor<TData>
        where TData : class, IData
    {
        #region Private Fields
        private Dictionary<Bus.Queue, Queue<TData>> _queues;
        private Queue<TData>[] _orderedQueues = Array.Empty<Queue<TData>>();
        private Dictionary<Type, Queue<TData>> _messageTypeQueues;
        private ILog _log;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _log);

            _queues = new Dictionary<Bus.Queue, Queue<TData>>();
            _messageTypeQueues = new Dictionary<Type, Queue<TData>>();
        }

        protected override void Release()
        {
            base.Release();

            _log = default;
            _queues = default;
            _messageTypeQueues = default;
        }
        #endregion

        #region Helper Methods
        public Boolean TryEnqueue(TData message)
        {
            if (_messageTypeQueues.TryGetValue(message.GetType(), out Queue<TData> queue))
            {
                queue.Enqueue(message);
                return true;
            }

            _log.Warn($"{nameof(MessageBus)}::{nameof(TryEnqueue)} - Unknown message type '{message.GetType().GetPrettyName()}'");
            return false;
        }

        public void ProcessEnqueued()
        {
            foreach (Queue<TData> queue in _orderedQueues)
            {
                while (queue.TryDequeue(out TData message))
                {
                    this.Process(message);
                }
            }
        }

        public Boolean TryRegisterQueue(Bus.Queue key, params Type[] messageTypes)
        {
            Queue<TData> queue = new Queue<TData>();
            if (_queues.TryAdd(key, queue))
            {
                // Attempt to order all registered queues into an array
                _orderedQueues = _queues.OrderBy(kvp => kvp.Key.Order).Select(kvp => kvp.Value).ToArray();

                // Create lookup table so that incoming messages can be placed into their apprioriate queue
                foreach (Type messageType in messageTypes)
                {
                    if (!_messageTypeQueues.TryAdd(messageType, queue))
                    {
                        _log.Warn($"{nameof(MessageBus)}::{nameof(TryRegisterQueue)} - Unable to register message type '{messageType.Name}' to queue '{key.Name}'.");
                    }
                }

                return true;
            }

            return false;
        }
        #endregion
    }
}
