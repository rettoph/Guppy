using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Collections;
using Lidgren.Network;

namespace Guppy.Network.Collections
{
    public class NetworkEntityCollection : NetworkObjectCollection<NetworkEntity>
    {
        public event EventHandler<NetworkEntity> Created;

        public NetworkEntityCollection(EntityCollection entities)
        {
            entities.Created += this.HandleEntityCreated;
            entities.Added += this.HandleEntityAdded;
            entities.Removed += this.HandleEntityRemoved;
        }

        #region Event Handlers
        private void HandleEntityCreated(object sender, Entity e)
        {
            if (e is NetworkEntity)
            { // If the new entity is a network entity...
                this.Created?.Invoke(this, e as NetworkEntity);
            }
        }

        private void HandleEntityAdded(object sender, Entity e)
        {
            if (e is NetworkEntity)
            { // If the new entity is a network entity...
                // Add it to the created entities queue
                this.Add(e as NetworkEntity);
            }
        }

        private void HandleEntityRemoved(object sender, Entity e)
        {
            if (e is NetworkEntity)
            { // If the new entity is a network entity...
                // Remove it to the network entities collection
                this.Remove(e as NetworkEntity);
            }
        }
        #endregion
    }
}
