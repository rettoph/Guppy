using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using Guppy.Network.Services;
using Guppy.Threading.Interfaces;
using Guppy.Threading.Utilities;
using Microsoft.Xna.Framework;
using Serilog;
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
        private Lazy<ILogger> _logger;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _entities);
            provider.ServiceLazy(out _logger);
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);
        }
        #endregion

        #region IMessageProcessor<CreateNetworkEntityMessage> Implementation
        Boolean IDataProcessor<TNetworkEntityMessage>.Process(TNetworkEntityMessage message)
        {
            // Create a new entity instance if one doesnt already exists...
            if (_entities.TryGetByNetworkId(message.NetworkId, out INetworkEntity entity) && entity is IMagicNetworkEntity magic)
            {
                magic.Messages.Process(message);
                return true;
            }

            _logger.Value.Verbose("{type}::{method} - Update to process {packet} message, an entity with the recieved {id} cannot be found.",
                nameof(NetworkEntityService),
                nameof(IDataProcessor<NetworkEntityMessage>.Process),
                message.GetType().GetPrettyName(),
                nameof(message.NetworkId));
            return false;
        }
        #endregion
    }
}
