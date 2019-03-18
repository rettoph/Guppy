using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Collections;

namespace Guppy.Network.Collections
{
    public class NetworkEntityCollection : NetworkObjectCollection<NetworkEntity>
    {
        public Queue<NetworkEntity> CreatedQueue;

        public NetworkEntityCollection(EntityCollection entities)
        {
            entities.Created += this.HandleEntityCreated;
            entities.Added += this.HandleEntityAdded;
            entities.Removed += this.HandleEntityRemoved;

            this.CreatedQueue = new Queue<NetworkEntity>();
        }

        #region Event Handlers
        private void HandleEntityCreated(object sender, Entity e)
        {
            if (e is NetworkEntity)
            { // If the new entity is a network entity...
                // Add it to the network entities collection
                this.CreatedQueue.Enqueue(e as NetworkEntity);
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
