using Guppy.EntityComponent.DependencyInjection;
using Guppy.Threading.Interfaces;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Utilities
{
    public class Bus : Bus<IData>
    {
    }

    public class Bus<TData> : DataProcessor<TData>
        where TData : class, IData
    {
        #region Private Fields
        private Dictionary<Int32, ConcurrentQueue<TData>> _queues;
        private ConcurrentQueue<TData>[] _orderedQueues = Array.Empty<ConcurrentQueue<TData>>();
        private Dictionary<Type, ConcurrentQueue<TData>> _messageTypeQueues;
        private ILogger _logger;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _logger);

            _queues = new Dictionary<Int32, ConcurrentQueue<TData>>();
            _messageTypeQueues = new Dictionary<Type, ConcurrentQueue<TData>>();
        }
        #endregion

        #region Helper Methods
        public Boolean TryEnqueue(TData message)
        {
            if (_messageTypeQueues.TryGetValue(message.GetType(), out ConcurrentQueue<TData> queue))
            {
                queue.Enqueue(message);
                return true;
            }

            _logger.Warning("{type}::{method} - Unknown message type '{input}'", nameof(MessageBus), nameof(TryEnqueue), message.GetType().GetPrettyName());
            return false;
        }

        public void ProcessEnqueued()
        {
            foreach (ConcurrentQueue<TData> queue in _orderedQueues)
            {
                while (queue.TryDequeue(out TData message))
                {
                    this.Process(message);
                }
            }
        }

        public void RegisterMessageTypes(Int32 queue, params Type[] messageTypes)
        {
            // Look i know theQueue isnt a great name. It makes sense that we call
            // the queue id 'queue' in the context of this method. it *is* queue N.
            // theQueue is.. the actual queue. Dont sue me
            if(!_queues.TryGetValue(queue, out ConcurrentQueue<TData> theQueue))
            {
                theQueue = new ConcurrentQueue<TData>();

                // Create the new queue
                _queues.Add(queue, theQueue);

                // Attempt to order all registered queues into an array
                _orderedQueues = _queues.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToArray();
            }

            // Create lookup table so that incoming messages can be placed into their apprioriate queue
            foreach (Type messageType in messageTypes)
            {
                if (_messageTypeQueues.TryAdd(messageType, theQueue))
                {
                    _logger.Information("{type}::{method} - Registered message type '{messageType}' to queue '{queue}'.", nameof(MessageBus), nameof(RegisterMessageTypes), messageType.Name, queue);
                    return;
                }

                _logger.Warning("{type}::{method} - Unable to register message type '{messageType}' to queue '{queue}'.", nameof(MessageBus), nameof(RegisterMessageTypes), messageType.Name, queue);
            }
        }
        #endregion
    }
}
