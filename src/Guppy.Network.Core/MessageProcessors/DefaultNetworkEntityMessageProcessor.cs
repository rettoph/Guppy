using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
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
    internal sealed class DefaultNetworkEntityMessageProcessor<TNetworkEntityMessage> : Service, IDataProcessor<TNetworkEntityMessage>
        where TNetworkEntityMessage : NetworkEntityMessage
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

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);
        }
        #endregion

        #region IMessageProcessor<CreateNetworkEntityMessage> Implementation
        Boolean IDataProcessor<TNetworkEntityMessage>.Process(TNetworkEntityMessage message)
        {
            return _entities.TryProcess(message);
        }
        #endregion
    }
}
