using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Threading.Interfaces;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Utilities
{
    public class MessageQueue : MessageQueue<IMessage>
    {
    }

    public class MessageQueue<TMessage> : Service
        where TMessage : class, IMessage
    {
        #region Classes
        private interface IMessageProcessorContainer
        {
            void Process(TMessage message);
        }

        private class MessageProcessorContainer<T> : IMessageProcessorContainer
            where T : class, TMessage
        {
            private delegate void ProcessDelegate(T message);

            private ProcessDelegate _processors;

            public MessageProcessorContainer(IMessageProcessor<T> processor)
            {
                _processors = processor.Process;
            }

            public void Process(TMessage message)
            {
                if (message is T casted)
                {
                    foreach(ProcessDelegate processor in _processors.GetInvocationList())
                    {
                        processor(casted);
                    }

                    return;
                }

                throw new ArgumentException(nameof(message));
            }

            public void RegisterProcessor(IMessageProcessor<T> processor)
            {
                _processors += processor.Process;
            }

            public void DeregisterProcessor(IMessageProcessor<T> processor)
            {
                _processors -= processor.Process;
            }
        }
        #endregion

        #region Private Fields
        private ILog _log;
        private Queue<TMessage> _queue;
        private Dictionary<Type, IMessageProcessorContainer> _processors;
        #endregion

        #region Constructors
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _log);

            _queue = new Queue<TMessage>();
            _processors = new Dictionary<Type, IMessageProcessorContainer>();
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            _queue.Clear();
            _processors.Clear();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Register a new processor into the queue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="processor"></param>
        public void RegisterProcessor<T>(IMessageProcessor<T> processor)
            where T : class, TMessage
        {
            if(_processors.TryGetValue(typeof(T), out IMessageProcessorContainer processors)
                && processors is MessageProcessorContainer<T> casted)
            {
                casted.RegisterProcessor(processor);
                return;
            }

            _processors.Add(typeof(T), new MessageProcessorContainer<T>(processor));
        }

        /// <summary>
        /// Deregister a processor from the queue.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="processor"></param>
        public void DeregisterProcessor<T>(IMessageProcessor<T> processor)
            where T : class, TMessage
        {
            if (_processors.TryGetValue(typeof(T), out IMessageProcessorContainer processors)
                && processors is MessageProcessorContainer<T> casted)
            {
                casted.DeregisterProcessor(processor);
                return;
            }
        }

        /// <summary>
        /// Process an incoming message immidiately.
        /// </summary>
        /// <param name="message"></param>
        public void Process(TMessage message)
        {
            if (_processors.TryGetValue(message.GetType(), out IMessageProcessorContainer processor))
            {
                processor.Process(message);
            }
            else
            {
                _log.Warn($"{this.GetType().GetPrettyName()}::{nameof(Process)} - Unknown type recieved:'{message.GetType().GetPrettyName()}'.");
            }
        }

        /// <summary>
        /// Process an incoming message immidiately.
        /// </summary>
        /// <param name="message"></param>
        public void Process(TMessage message, Action<TMessage> postProcessor)
        {
            if (_processors.TryGetValue(message.GetType(), out IMessageProcessorContainer processor))
            {
                processor.Process(message);
            }
            else
            {
                throw new KeyNotFoundException($"{this.GetType().GetPrettyName()}::{nameof(Process)} - Unknown type recieved when processing message:'{message}'.");
            }

            postProcessor(message);
        }

        /// <summary>
        /// Enqueue an incoming message to be processed once 
        /// <see cref="ProcessEnqueued"/> is called
        /// </summary>
        /// <param name="message"></param>
        public void Enqueue(TMessage message)
        {
            _queue.Enqueue(message);
        }

        /// <summary>
        /// Process all enqueued Dtos.
        /// </summary>
        public void ProcessEnqueued()
        {
            while(_queue.TryDequeue(out TMessage message))
            {
                this.Process(message);
            }
        }

        /// <summary>
        /// Process all enqueued Dtos.
        /// </summary>
        public void ProcessEnqueued(Action<TMessage> postProcessor)
        {
            while (_queue.TryDequeue(out TMessage message))
            {
                this.Process(message, postProcessor);
            }
        }
        #endregion
    }
}
