﻿using Guppy.EntityComponent;
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

        internal Boolean TryProcess(NetworkEntityMessage message)
        {
            // Create a new entity instance if one doesnt already exists...
            if (_entities.TryGetValue(message.NetworkId, out INetworkEntity entity))
            {
                this.ProcessPackets(entity, message);
                return true;
            }
            else
            {
                _log.Warn($"{nameof(NetworkEntityService)}::{nameof(TryProcess)} - Update to process {message.GetType().GetPrettyName()} message, an entity with the recieved {nameof(message.NetworkId)} cannot be found.");
                return false;
            }
        }

        internal Boolean TryProcess(CreateNetworkEntityMessage message)
        {
            // Create a new entity instance if one doesnt already exists...
            if(!_entities.TryGetValue(message.NetworkId, out INetworkEntity entity))
            { // No entity with the recieved id exists, try creating one now!
                entity = _provider.GetService<INetworkEntity>(message.ServiceConfigurationId, (e, _, _) =>
                { // Set the entity's internal network id to the message value.
                    e.NetworkId = message.NetworkId;
                    this.ProcessPackets(e, message);
                });
            }
            else if(entity.ServiceConfiguration.Id != message.ServiceConfigurationId)
            {
                _log.Warn($"{nameof(NetworkEntityService)}::{nameof(TryProcess)} - Update to process {nameof(CreateNetworkEntityMessage)} message, an entity with the recieved {nameof(CreateNetworkEntityMessage.NetworkId)} already exists, but the expected {nameof(CreateNetworkEntityMessage.ServiceConfigurationId)} does not match.");
                return false;
            }
            else
            {
                this.ProcessPackets(entity, message);
            }

            // Entity created, we can safely assume it was added?
            return true;
        }

        private void ProcessPackets(INetworkEntity entity, NetworkEntityMessage message)
        {
            foreach (IPacket packet in message.Packets)
            {
                entity.Packets.Process(packet);
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
    }
}