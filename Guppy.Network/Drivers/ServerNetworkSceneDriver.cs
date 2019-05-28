using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Network.Collections;
using Guppy.Network.Groups;
using Guppy.Network.Interfaces;
using Guppy.Network.Security;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Guppy.Network.Extensions.Lidgren;

namespace Guppy.Network.Drivers
{
    public class ServerNetworkSceneDriver : NetworkSceneDriver
    {
        private NetworkEntityCollection _networkEntities;
        private Queue<NetworkEntity> _dirtyEntityQueue;
        private Queue<NetOutgoingMessage> _createdMessageQueue;
        private ServerGroup _group;

        public ServerNetworkSceneDriver(NetworkEntityCollection networkEntities, NetworkScene scene, IServiceProvider provider, ILogger logger) : base(scene, provider, logger)
        {
            _networkEntities = networkEntities;
        }

        protected override void Boot()
        {
            base.Boot();

            _dirtyEntityQueue = new Queue<NetworkEntity>();
            _createdMessageQueue = new Queue<NetOutgoingMessage>();
            _group = this.scene.Group as ServerGroup;

            _networkEntities.Created += this.HandleNetworkEntityCreated;
            _networkEntities.Removed += this.HandleNetworkEntityRemoved;
            this.scene.Group.Users.Added += this.HandleUserAdded;
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
                    this.scene.Group.SendMesssage(this.BuildUpdateMessage(_dirtyEntityQueue.Dequeue()), NetDeliveryMethod.ReliableOrdered);
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
            _createdMessageQueue.Enqueue(this.BuildCreateMessage(e));

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

        /// <summary>
        /// When a new user joins the server, send them all entities 
        /// complete with update data. This assumes the client is using
        /// the predefined ClientNetworkSceneDriver to handle all these
        /// messages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="user"></param>
        private void HandleUserAdded(object sender, User user)
        {
            /*
             * BEGIN NEW USER SETUP
             */
            // Cache all network entities as is
            var networkEntities = _networkEntities.ToArray();

            // Send setup begin message to new user...
            _group.SendMesssage(this.scene.Group.CreateMessage("setup:begin"), user, NetDeliveryMethod.ReliableOrdered);

            foreach (NetworkEntity ne in networkEntities.OrderBy(ne => ne.UpdateOrder))
            {
                _group.SendMesssage(this.BuildCreateMessage(ne), user, NetDeliveryMethod.ReliableOrdered);
                _group.SendMesssage(this.BuildUpdateMessage(ne), user, NetDeliveryMethod.ReliableOrdered);
            }

            // Send setup end message to new user...
            _group.SendMesssage(this.scene.Group.CreateMessage("setup:end"), user, NetDeliveryMethod.ReliableOrdered);
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();

            _networkEntities.Created -= this.HandleNetworkEntityCreated;
            _networkEntities.Removed -= this.HandleNetworkEntityRemoved;
        }

        #region Network Entity Message Builders

        public NetOutgoingMessage BuildCreateMessage(NetworkEntity ne)
        {
            var om = this.scene.Group.CreateMessage("create");
            om.Write(ne.Configuration.Handle);
            om.Write(ne.Id);

            return om;
        }

        public NetOutgoingMessage BuildUpdateMessage(NetworkEntity ne)
        {
            var om = this.scene.Group.CreateMessage("update");
            om.Write(ne.Id);
            ne.Write(om);

            return om;
        }
        #endregion
    }
}
