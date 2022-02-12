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
    internal sealed class CreateDisposeNetworkEntityMessageProcessor : Service, 
        IDataProcessor<CreateNetworkEntityMessage>,
        IDataProcessor<DisposeNetworkEntityMessage>
    {
        #region Private Fields
        private NetworkEntityService _entities;
        private ServiceProvider _provider;
        private ILogger _logger;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            provider.Service(out _entities);
        }
        #endregion

        #region IMessageProcessor<CreateNetworkEntityMessage> Implementation
        Boolean IDataProcessor<CreateNetworkEntityMessage>.Process(CreateNetworkEntityMessage message)
        {
            // Create a new entity instance if one doesnt already exists...
            if (!_entities.TryGetByNetworkId(message.NetworkId, out INetworkEntity entity))
            { // No entity with the recieved id exists, try creating one now!
                entity = _provider.GetService<IMagicNetworkEntity>(message.ServiceConfigurationId, (e, _, _) =>
                { // Set the entity's internal network id to the message value.
                    e.NetworkId = message.NetworkId;
                    e.Messages.Process(message);
                });
            }
            else if (entity.ServiceConfiguration.Id != message.ServiceConfigurationId)
            {
                _logger.Warning($"{nameof(NetworkEntityService)}::{nameof(IDataProcessor<CreateNetworkEntityMessage>.Process)} - Update to process {nameof(CreateNetworkEntityMessage)} message, an entity with the recieved {nameof(CreateNetworkEntityMessage.NetworkId)} already exists, but the expected {nameof(CreateNetworkEntityMessage.ServiceConfigurationId)} does not match.");
                return false;
            }
            else if (entity is IMagicNetworkEntity magic)
            {
                magic.Messages.Process(message);
            }

            // Entity created, we can safely assume it was added?
            return true;
        }

        Boolean IDataProcessor<DisposeNetworkEntityMessage>.Process(DisposeNetworkEntityMessage message)
        {
            // Try to load the requested entity...
            if (_entities.TryGetByNetworkId(message.NetworkId, out INetworkEntity entity) && entity is IMagicNetworkEntity magic)
            {
                // Do one final process, just in case...
                magic.Messages.Process(message);
                // Goodbye.
                entity.Dispose();
                return true;
            }

            return false;
        }
        #endregion
    }
}
