using Guppy.Network.Configurations;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    /// <summary>
    /// Pool containing outgoing message configuration objects
    /// This allows for the reuse of said objects.
    /// </summary>
    public class NetOutgoingMessageConfigurationPool
    {
        private Queue<NetOutgoingMessageConfiguration> _pool;

        public NetOutgoingMessageConfigurationPool()
        {
            _pool = new Queue<NetOutgoingMessageConfiguration>();
        }

        /// <summary>
        /// Enqueue a net outgoing message configuration instance
        /// </summary>
        /// <param name="configuration"></param>
        public void Put(NetOutgoingMessageConfiguration configuration)
        {
            _pool.Enqueue(configuration);
        }

        /// <summary>
        /// Return a new configuration instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="method"></param>
        /// <param name="sequenceChannel"></param>
        /// <returns></returns>
        public NetOutgoingMessageConfiguration Pull(NetOutgoingMessage message, NetConnection target, NetDeliveryMethod method, Int32 sequenceChannel)
        {
            if (_pool.Count == 0)
                this.Create();

            var config = _pool.Dequeue();

            config.Message = message;
            config.Target = target;
            config.Method = method;
            config.SequenceChannel = sequenceChannel;

            return config;
        }

        protected void Create()
        {
            _pool.Enqueue(new NetOutgoingMessageConfiguration());
        }
    }
}
