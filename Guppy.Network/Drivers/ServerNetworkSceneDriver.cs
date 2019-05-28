using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Collections;
using Guppy.Network.Interfaces;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;

namespace Guppy.Network.Drivers
{
    public class ServerNetworkSceneDriver : NetworkSceneDriver
    {
        private NetworkEntityCollection _networkEntities;

        private Queue<NetworkEntity> _dirtyEntityQueue;
        private Queue<NetOutgoingMessage> _createdMessageQueue;

        public ServerNetworkSceneDriver(NetworkEntityCollection networkEntities, NetworkScene scene, IServiceProvider provider, ILogger logger) : base(scene, provider, logger)
        {
            _networkEntities = networkEntities;
        }

        protected override void Boot()
        {
            base.Boot();

            _dirtyEntityQueue = new Queue<NetworkEntity>();
            _createdMessageQueue = new Queue<NetOutgoingMessage>();

            _networkEntities.Created += this.HandleNetworkEntityCreated;
            _networkEntities.Removed += this.HandleNetworkEntityRemoved;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.scene.Group.Users.Count() > 0)
            {
                // Push all create messages to the client
                while (_createdMessageQueue.Count > 0)
                    this.scene.Group.SendMesssage(_createdMessageQueue.Dequeue(), NetDeliveryMethod.ReliableOrdered);

                // Push all update messages to the client
                while (_dirtyEntityQueue.Count > 0)
                    this.scene.Group.SendMesssage(_dirtyEntityQueue.Dequeue().BuildUpdateMessage(), NetDeliveryMethod.ReliableSequenced);
            }
            else
            {
                _createdMessageQueue.Clear();
                _dirtyEntityQueue.Clear();
            }
        }

        #region Event Handlers
        private void HandleNetworkEntityCreated(object sender, NetworkEntity e)
        {
            _createdMessageQueue.Enqueue(e.BuildCreateMessage());

            e.Dirty = true;
            _dirtyEntityQueue.Enqueue(e);

            e.OnDirtyChanged += this.HandleNetworkEntityDirtyChanged;
        }

        private void HandleNetworkEntityRemoved(object sender, NetworkEntity e)
        {
            e.OnDirtyChanged -= this.HandleNetworkEntityDirtyChanged;
        }

        private void HandleNetworkEntityDirtyChanged(object sender, ITrackedNetworkObject e)
        {
            if (e.Dirty)
                _dirtyEntityQueue.Enqueue(e as NetworkEntity);
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();

            _networkEntities.Created -= this.HandleNetworkEntityCreated;
            _networkEntities.Removed -= this.HandleNetworkEntityRemoved;
        }
    }
}
