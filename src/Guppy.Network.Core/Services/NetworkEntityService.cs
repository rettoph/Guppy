using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Lists;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using Guppy.Threading.Interfaces;
using Guppy.Threading.Utilities;
using Microsoft.Xna.Framework;
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
            if (_entities.TryGetValue(networkId, out INetworkEntity uncasted) && uncasted is TNetworkEntity casted)
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
