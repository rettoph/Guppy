using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using Guppy.Network.Services;
using Guppy.Threading.Interfaces;
using Guppy.Threading.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.MessageProcessors
{
    internal sealed class CreateDisposeNetworkEntityMessageProcessor : Service, 
        IDataProcessor<CreateNetworkEntityMessage>,
        IDataProcessor<DisposeNetworkEntityMessage>
    {
        #region Private Fields
        private NetworkEntityService _entities;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _entities);
        }
        #endregion

        #region IMessageProcessor<CreateNetworkEntityMessage> Implementation
        Boolean IDataProcessor<CreateNetworkEntityMessage>.Process(CreateNetworkEntityMessage message)
        {
            return _entities.TryProcess(message);
        }

        Boolean IDataProcessor<DisposeNetworkEntityMessage>.Process(DisposeNetworkEntityMessage message)
        {
            return _entities.TryProcess(message);
        }
        #endregion
    }
}
