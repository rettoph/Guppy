using Guppy.Network.Collections;
using Guppy.Network.Groups;
using Guppy.Network.Security;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    public abstract class NetworkScene : Scene
    {
        private Queue<NetworkEntity> _createdEntities;
        private NetworkObjectCollection<NetworkEntity> _networkEntities;
        protected Group group;

        public NetworkScene(IServiceProvider provider) : base(provider)
        {
        }

        #region Initialization Methods
        protected override void PreInitialize()
        {
            base.PreInitialize();

            _createdEntities = new Queue<NetworkEntity>();
            _networkEntities = new NetworkObjectCollection<NetworkEntity>();

            this.entities.Created += this.HandleEntityCreated;
            this.entities.Added += this.HandleEntityAdded;
            this.entities.Removed += this.HandleEntityRemoved;
        }
        #endregion

        #region Frame Methods
        public override void Update(GameTime gameTime)
        {
            // First send all create method messages
            while(_createdEntities.Count > 0)
                this.group.SendMesssage(
                    _createdEntities.Dequeue().BuildCreateMessage(this.group),
                    NetDeliveryMethod.ReliableOrdered,
                    0);

            base.Update(gameTime);

            // Finally send all update method messages
            while (_networkEntities.DirtyQueue.Count > 0)
                this.group.SendMesssage(
                    _networkEntities.DirtyQueue.Dequeue().BuildUpdateMessage(this.group),
                    NetDeliveryMethod.ReliableOrdered,
                    0);
        }
        #endregion

        #region Event Handlers
        private void HandleEntityCreated(object sender, Entity e)
        {
            if(e is NetworkEntity)
            { // If the new entity is a network entity...
                // Add it to the network entities collection
                _createdEntities.Enqueue(e as NetworkEntity);
            }
        }

        private void HandleEntityAdded(object sender, Entity e)
        {
            if (e is NetworkEntity)
            { // If the new entity is a network entity...
                // Add it to the created entities queue
                _networkEntities.Add(e as NetworkEntity);
            }
        }

        private void HandleEntityRemoved(object sender, Entity e)
        {
            if (e is NetworkEntity)
            { // If the new entity is a network entity...
                // Remove it to the network entities collection
                _networkEntities.Remove(e as NetworkEntity);
            }
        }
        #endregion
    }
}
