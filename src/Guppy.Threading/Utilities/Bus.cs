using Guppy.EntityComponent.DependencyInjection;
using Guppy.Threading.Interfaces;
using Microsoft.Xna.Framework;
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

    public class Bus<TData> : Publisher<TData>
        where TData : class, IData
    {
        #region Classes
        public class Queue
        {
            private UInt32 _maximumMessagesPerSecond;
            private ConcurrentQueue<TData> _queue;
            private HashSet<Type> _types;
            private Bus<TData> _bus;

            public Int32 Index { get; private set; }
            public UInt32 MaximumMessagesPerSecond
            {
                get => _maximumMessagesPerSecond;
                set => _maximumMessagesPerSecond = value;
            }

            internal Queue(Bus<TData> bus, Int32 index)
            {
                this.Index = index;
                this.MaximumMessagesPerSecond = 0;

                _queue = new ConcurrentQueue<TData>();
                _types = new HashSet<Type>();
                _bus = bus;
            }

            public Queue SetMaximumMessagesPerSecond(UInt32 maximumMessagesPerSecond)
            {
                _maximumMessagesPerSecond = maximumMessagesPerSecond;
                return this;
            }

            public Queue RegisterType<T>(Boolean deregisterIfNecessary = false)
                where T : TData
            {
                return this.RegisterType(typeof(T), deregisterIfNecessary);
            }

            public Queue DeregisterType<T>()
                where T : TData
            {
                return this.DeregisterType(typeof(T));
            }

            public Queue RegisterType(Type type, Boolean deregisterIfNecessary = false)
            {
                typeof(TData).ValidateAssignableFrom(type);

                // The message has already been registered! 
                if(_bus._messageTypeQueues.TryGetValue(type, out Queue oldQueue) && oldQueue != this)
                {
                    if(deregisterIfNecessary)
                    {
                        oldQueue.DeregisterType(type);
                    }
                    else
                    {
                        throw new InvalidOperationException($"{nameof(Queue)}::{nameof(RegisterType)} - Unable to register type '{type.Name}' to queue '{this.Index}', as it is already registered to queue {oldQueue.Index}");
                    }
                }
                
                if(oldQueue == this)
                {
                    _bus._logger.Warning("{type}::{method} - Message type '{messageType}' has already been registered.", nameof(Queue), nameof(RegisterType), type.Name);
                    return this;
                }

                _types.Add(type);
                _bus._messageTypeQueues[type] = this;
                _bus._logger.Information("{type}::{method} - Registered message type '{messageType}' to queue '{queue}'", nameof(Queue), nameof(RegisterType), type.Name, this.Index);

                return this;
            }

            public Queue DeregisterType(Type type)
            {
                if (!_bus._messageTypeQueues.TryGetValue(type, out Queue queue))
                {
                    _bus._logger.Warning("{type}::{method} - Message type '{messageType}' is not registered.", nameof(Queue), nameof(DeregisterType), type.Name);
                    return this;
                }

                if (queue != this)
                {
                    _bus._logger.Warning("{type}::{method} - Message type '{messageType}' is not registered to the current queue..", nameof(Queue), nameof(DeregisterType), type.Name);
                    return this;
                }

                _types.Remove(type);
                _bus._messageTypeQueues.Remove(type);
                _bus._logger.Information("{type}::{method} - Deregistered message type '{messageType}' from queue '{queue}'", nameof(Queue), nameof(RegisterType), type.Name, this.Index);

                return this;
            }

            public void RegisterTypes(Type[] types, Boolean deregisterIfNecessary = false)
            {
                foreach(Type type in types)
                {
                    this.RegisterType(type, deregisterIfNecessary);
                }
            }

            public void Enqueue(TData data)
            {
                _queue.Enqueue(data);
            }

            internal void Publish()
            {
                while (_queue.TryDequeue(out TData message))
                {
                    _bus.Publish(message);
                }
            }

            internal void Publish(GameTime gameTime)
            {
                if(_maximumMessagesPerSecond == 0)
                {
                    this.Publish();
                    return;
                }

                var messagesToPublishThisFrame = (UInt32)(gameTime.ElapsedGameTime.TotalMilliseconds * _maximumMessagesPerSecond) + 1;
                var messagesPublished = 0;

                while (++messagesPublished < messagesToPublishThisFrame && _queue.TryDequeue(out TData message))
                {
                    _bus.Publish(message);
                }
            }
        }
        #endregion

        #region Constants
        public const Int32 DefaultQueue = 0;
        #endregion

        #region Private Fields
        private Dictionary<Int32, Queue> _queues;
        private Queue[] _orderedQueues = Array.Empty<Queue>();
        private Dictionary<Type, Queue> _messageTypeQueues;
        private Queue _defaultQueue;
        private ILogger _logger;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _logger);

            _queues = new Dictionary<Int32, Queue>();
            _messageTypeQueues = new Dictionary<Type, Queue>();
            _defaultQueue = this.GetQueue(Bus.DefaultQueue);
        }
        #endregion

        #region Helper Methods
        public void Enqueue(TData message)
        {
            if (_messageTypeQueues.TryGetValue(message.GetType(), out Queue queue))
            {
                queue.Enqueue(message);
                return;
            }

            _defaultQueue.Enqueue(message);
        }

        public void PublishEnqueued()
        {
            foreach (Queue queue in _orderedQueues)
            {
                queue.Publish();
            }
        }


        public void PublishEnqueued(GameTime gameTime)
        {
            foreach (Queue queue in _orderedQueues)
            {
                queue.Publish(gameTime);
            }
        }

        public Queue GetQueue(Int32 index)
        {
            if (!_queues.TryGetValue(index, out Queue queue))
            {
                queue = new Queue(this, index);

                // Create the new queue
                _queues.Add(queue.Index, queue);

                // Attempt to order all registered queues into an array
                _orderedQueues = _queues.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToArray();
            }

            return queue;
        }
        #endregion
    }
}
