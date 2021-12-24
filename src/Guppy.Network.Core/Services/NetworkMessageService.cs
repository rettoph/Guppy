using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Configurations;
using Guppy.Network.Interfaces;
using Guppy.Threading.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    public sealed class NetworkMessageService : Service
    {
        #region Private Fields
        private MessageQueue<IData> _incomingMessageQueue;
        private NetworkProvider _network;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _network);
            provider.Service(out _incomingMessageQueue);

            // Iterate through each valid configuration and register a processor for it
            IEnumerable<NetworkMessageConfiguration> configurations = _network.MessageConfigurations.Where(mc => mc.Filter(provider, mc));
            foreach (NetworkMessageConfiguration configuration in configurations)
            {
                configuration.TryRegisterProcessor(provider, _incomingMessageQueue);
            }
        }
        #endregion

        #region Helper Methods
        public void Enqueue(IData data)
        {
            _incomingMessageQueue.Enqueue(data);
        }

        public void ProcessEnqueued()
        {
            _incomingMessageQueue.ProcessEnqueued();
        }
        #endregion
    }
}
