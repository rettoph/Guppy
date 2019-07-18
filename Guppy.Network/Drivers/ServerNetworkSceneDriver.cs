using Guppy.Network.Groups;
using Guppy.Network.Security;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Collections;
using Guppy.Network.Peers;

namespace Guppy.Network.Drivers
{
    public class ServerNetworkSceneDriver : NetworkSceneDriver
    {
        #region Private Fields
        private ServerGroup _group;
        private ServerPeer _server;
        private NetworkEntityCollection _networkEntities;
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

            _group.OnSetup += this.HandleSetup;
            _networkEntities.Added += this.HandleNetworkEntityCreated;
        }
        #endregion

        #region Event Handlers
        private void HandleSetup(object sender, User e)
        {
            var connection = _server.GetNetConnectionByUser(e);

            // Send the new user create commands for all existing objects
            foreach(NetworkEntity ne in _networkEntities)
            {
                this.CreateCreateMessage(ne, connection);
            }
        }

        private void HandleNetworkEntityCreated(object sender, NetworkEntity ne)
        {
            this.CreateCreateMessage(ne);
        }
        #endregion

        #region Utility Methods
        public void CreateCreateMessage(NetworkEntity ne, NetConnection target = null)
        {
            var om = _group.CreateMessage("create", NetDeliveryMethod.ReliableOrdered, 0, target);
            om.Write(ne.Configuration.Handle);
            om.Write(ne.Id);

            this.CreateUpdateMessage(ne, target);
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
