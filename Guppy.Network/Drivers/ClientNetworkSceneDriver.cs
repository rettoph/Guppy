using Guppy.Collections;
using Guppy.Network.Extensions.Lidgren;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Drivers
{
    public class ClientNetworkSceneDriver : NetworkSceneDriver
    {
        #region Private Fields
        private EntityCollection _entities;
        #endregion

        #region Constructors
        public ClientNetworkSceneDriver(EntityCollection entities, NetworkScene scene, IServiceProvider provider) : base(scene, provider)
        {
            _entities = entities;
        }
        #endregion

        #region Initialization Methods
        protected override void Initialize()
        {
            base.Initialize();

            this.scene.Group.AddMessageHandler("create", this.HandleCreateMessage);
            this.scene.Group.AddMessageHandler("update", this.HandleUpdateMessage);
        }

        private void HandleUpdateMessage(NetIncomingMessage obj)
        {
            // Load the target entity and read the data from the incoming message to it
            (_entities.GetById(obj.ReadGuid()) as NetworkEntity).Read(obj);
        }

        private void HandleCreateMessage(NetIncomingMessage obj)
        {
            // Create a new entity based on the server commands
            _entities.Create(obj.ReadString(), obj.ReadGuid());
        }
        #endregion
    }
}
