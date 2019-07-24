using Guppy.Network.Groups;
using Guppy.Network.Security;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Collections;
using Guppy.Network.Peers;
using Guppy.Network.Interfaces;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;

namespace Guppy.Network.Drivers
{
    public class ServerNetworkSceneDriver : NetworkSceneDriver
    {
        #region Private Fields
        private ServerGroup _group;
        private ServerPeer _server;
        private NetworkEntityCollection _networkEntities;
        private Queue<NetworkEntity> _dirtyEntities;
        private NetworkEntity _o;
        #endregion

        #region Construcor
        public ServerNetworkSceneDriver(ServerPeer server, NetworkEntityCollection networkEntities, NetworkScene scene, IServiceProvider provider) : base(scene, provider)
        {
            _server = server;
            _networkEntities = networkEntities;
        }
        #endregion

        #region Initialization Methods
        protected override void Initialize()
        {
            base.Initialize();

            _group = this.scene.Group as ServerGroup;
            _dirtyEntities = new Queue<NetworkEntity>();

            _group.OnSetup += this.HandleSetup;
            _networkEntities.Created += this.HandleNetworkEntityCreated;
        }
        #endregion

        #region Frame Methods
        protected override void update(GameTime gameTime)
        {
            base.update(gameTime);

            // Send update message for enqueued network entities.
            while (_dirtyEntities.Count > 0)
            {
                // Update and clean any dirty network entites
                _o = _dirtyEntities.Dequeue();
                this.CreateUpdateMessage(_o);
                _o.Dirty = false;
            }
                
        }
        #endregion

        #region Event Handlers
        private void HandleSetup(object sender, User e)
        {
            var connection = _server.GetNetConnectionByUser(e);

            // Send the new user create commands for all existing objects
            foreach(NetworkEntity ne in _networkEntities)
                this.CreateCreateMessage(ne, connection, false);
            foreach (NetworkEntity ne in _networkEntities)
                this.CreateUpdateMessage(ne, connection);
        }

        private void HandleNetworkEntityCreated(object sender, NetworkEntity ne)
        {
            this.CreateCreateMessage(ne);
            
            // Setup event handlers
            ne.OnDirtyChanged += this.HandleNetworkEntityDirtyChanged;
            ne.Disposing += this.HandleNetworkEntityDisposing;
        }

        private void HandleNetworkEntityDirtyChanged(object sender, ITrackedNetworkObject e)
        {
            if (e.Dirty) // Queue the entity for cleaning
                _dirtyEntities.Enqueue(e as NetworkEntity);
        }

        private void HandleNetworkEntityDisposing(object sender, ITrackedDisposable e)
        {
            // Unbind any added event handlers
            (e as NetworkEntity).OnDirtyChanged -= this.HandleNetworkEntityDirtyChanged;
            (e as NetworkEntity).Disposing -= this.HandleNetworkEntityDisposing;
        }
        #endregion

        #region Utility Methods
        public void CreateCreateMessage(NetworkEntity ne, NetConnection target = null, Boolean full = true)
        {
            var om = _group.CreateMessage("create", NetDeliveryMethod.ReliableOrdered, 0, target);
            om.Write(ne.Configuration.Handle);
            om.Write(ne.Id);

            if(om.WriteIf(full))
                ne.Write(om);
        }

        public void CreateUpdateMessage(NetworkEntity ne, NetConnection target = null)
        {
            var om = _group.CreateMessage("update", NetDeliveryMethod.ReliableOrdered, 0, target);
            om.Write(ne.Id);
            ne.Write(om);
        }
        #endregion
    }
}
