using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Lists;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using Guppy.Threading.Interfaces;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    /// <summary>
    /// Manage scoped NetworkEntity instances.
    /// </summary>
    public class NetworkEntityService : Service, IEnumerable<INetworkEntity>
    {
        private ILogger _logger;
        private ServiceProvider _provider;
        private Dictionary<UInt16, INetworkEntity> _entities;

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            _entities = new Dictionary<UInt16, INetworkEntity>();

            _provider.Service(out _logger);
        }

        protected override void PostUninitialize()
        {
            base.PostUninitialize();

            _entities.Clear();
        }
        #endregion

        #region Helper Methods
        public Boolean TryGetByNetworkId<TNetworkEntity>(UInt16 networkId, out TNetworkEntity entity)
            where TNetworkEntity : INetworkEntity
        {
            if(_entities.TryGetValue(networkId, out INetworkEntity uncasted) && uncasted is TNetworkEntity casted)
            {
                entity = casted;
                return true;
            }

            entity = default;
            return false;
        }
        #endregion

        #region IEnumerable<INetworkEntity> Implementation
        public IEnumerator<INetworkEntity> GetEnumerator()
        {
            return _entities.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region Internal Methods
        internal Boolean TryProcess(NetworkEntityMessage message)
        {
            // Create a new entity instance if one doesnt already exists...
            if (_entities.TryGetValue(message.NetworkId, out INetworkEntity entity) && entity is IMagicNetworkEntity magic)
            {
                this.ProcessPackets(magic, message);
                return true;
            }
            else
            {
                _logger.Warning($"{nameof(NetworkEntityService)}::{nameof(TryProcess)} - Update to process {message.GetType().GetPrettyName()} message, an entity with the recieved {nameof(message.NetworkId)} cannot be found.");
                return false;
            }
        }

        internal Boolean TryProcess(CreateNetworkEntityMessage message)
        {
            // Create a new entity instance if one doesnt already exists...
            if(!_entities.TryGetValue(message.NetworkId, out INetworkEntity entity))
            { // No entity with the recieved id exists, try creating one now!
                entity = _provider.GetService<IMagicNetworkEntity>(message.ServiceConfigurationId, (e, _, _) =>
                { // Set the entity's internal network id to the message value.
                    e.NetworkId = message.NetworkId;
                    this.ProcessPackets(e, message);
                });
            }
            else if(entity.ServiceConfiguration.Id != message.ServiceConfigurationId)
            {
                _logger.Warning($"{nameof(NetworkEntityService)}::{nameof(TryProcess)} - Update to process {nameof(CreateNetworkEntityMessage)} message, an entity with the recieved {nameof(CreateNetworkEntityMessage.NetworkId)} already exists, but the expected {nameof(CreateNetworkEntityMessage.ServiceConfigurationId)} does not match.");
                return false;
            }
            else if (entity is IMagicNetworkEntity magic)
            {
                this.ProcessPackets(magic, message);
            }

            // Entity created, we can safely assume it was added?
            return true;
        }

        internal Boolean TryProcess(DisposeNetworkEntityMessage message)
        {
            // Try to load the requested entity...
            if (_entities.TryGetValue(message.NetworkId, out INetworkEntity entity) && entity is IMagicNetworkEntity magic)
            {
                // Do one final process, just in case...
                this.ProcessPackets(magic, message);
                // Goodbye.
                entity.Dispose();
                return true;
            }

            return false;
        }

        private void ProcessPackets(IMagicNetworkEntity entity, NetworkEntityMessage message)
        {
            entity.Messages.Publish(message);

            foreach (IData packet in message.Packets)
            {
                entity.Messages.Publish(packet);
            }
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
        #endregion


    }
}
