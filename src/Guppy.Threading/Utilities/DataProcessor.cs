using Guppy.EntityComponent;
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
    public class DataProcessor<TData> : Service
        where TData : class, IData
    {
        #region Classes
        private interface IMessageProcessorContainer
        {
            void Process(TData message);
        }

        private class MessageProcessorContainer<T> : IMessageProcessorContainer
            where T : class, TData
        {
            private delegate void ProcessDelegate(T message);

            private ProcessDelegate _processors;

            public MessageProcessorContainer(IDataProcessor<T> processor)
            {
                _processors = processor.Process;
            }

            public void Process(TData message)
            {
                if (message is T casted)
                {
                    foreach (ProcessDelegate processor in _processors.GetInvocationList())
                    {
                        processor(casted);
                    }

                    return;
                }

                throw new ArgumentException(nameof(message));
            }

            public void RegisterProcessor(IDataProcessor<T> processor)
            {
                _processors += processor.Process;
            }

            public void DeregisterProcessor(IDataProcessor<T> processor)
            {
                _processors -= processor.Process;
            }
        }
        #endregion

        #region Private Fields
        private ILog _log;
        private Dictionary<Type, IMessageProcessorContainer> _processors;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _log);

            _processors = new Dictionary<Type, IMessageProcessorContainer>();
        }

        protected override void Release()
        {
            base.Release();

            _log = default;
            _processors = default;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Register a new processor into the queue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="processor"></param>
        public void RegisterProcessor<T>(IDataProcessor<T> processor)
            where T : class, TData
        {
            if (_processors.TryGetValue(typeof(T), out IMessageProcessorContainer processors)
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
        public void DeregisterProcessor<T>(IDataProcessor<T> processor)
            where T : class, TData
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
        public void Process(TData message)
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
        #endregion
    }
}
