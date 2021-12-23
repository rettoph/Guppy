using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Lists;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    /// <summary>
    /// Manage scoped NetworkEntity instances.
    /// </summary>
    public class NetworkEntityService : Service
    {
        private ILog _log;
        private ServiceProvider _provider;
        private Dictionary<UInt16, INetworkEntity> _entities;

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            _entities = new Dictionary<ushort, INetworkEntity>();

            _provider.Service(out _log);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            _entities.Clear();
            _log = default;
            _provider = default;
        }
        #endregion

        internal Boolean TryCreate(CreateNetworkEntityMessage message, out INetworkEntity entity)
        {
            // Create a new entity instance if one doesnt already exists...
            if(!_entities.TryGetValue(message.NetworkId, out entity))
            { // No entity with the recieved id exists, try creating one now!
                entity = _provider.GetService<INetworkEntity>(message.ServiceConfigurationId, (e, _, _) =>
                { // Set the entity's internal network id to the message value.
                    e.NetworkId = message.NetworkId;
                });
            }
            else if(entity.ServiceConfiguration.Id != message.ServiceConfigurationId)
            {
                _log.Warn($"{nameof(NetworkEntityService)}::{nameof(TryCreate)} - Update to process {nameof(CreateNetworkEntityMessage)} message, as an entity with the recieved {nameof(CreateNetworkEntityMessage.NetworkId)} already exists, but the expected {nameof(CreateNetworkEntityMessage.ServiceConfigurationId)} does not match.");
                return false;
            }

            // Entity created, we can safely assume it was added?
            return true;
        }

        /// <summary>
        /// Attempt to add a new entity into the current serivce...
        /// </summary>
        /// <param name="entity"></param>
        internal Boolean TryAdd(INetworkEntity entity)
        {
            return _entities.TryAdd(entity.NetworkId, entity);
        }

        internal void Remove(INetworkEntity entity)
        {
            _entities.Remove(entity.NetworkId);
        }
    }
}
