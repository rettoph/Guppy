using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Collections;
using Guppy.Network.Collections;
using Guppy.Network.Extensions.Lidgren;
using Lidgren.Network;
using Microsoft.Extensions.Logging;

namespace Guppy.Network.Drivers
{
    public class ClientNetworkSceneDriver : NetworkSceneDriver
    {
        private NetworkEntityCollection _networkEntities;
        private EntityCollection _entities;
        private Queue<NetIncomingMessage> _updateMessageQueue;
        private Queue<NetIncomingMessage> _actionMessageQueue;

        public ClientNetworkSceneDriver(NetworkScene scene, NetworkEntityCollection networkEntities, EntityCollection entities, IServiceProvider provider) : base(scene, provider)
        {
            _networkEntities = networkEntities;
            _entities = entities;
        }

        protected override void Boot()
        {
            base.Boot();

            _updateMessageQueue = new Queue<NetIncomingMessage>();
            _actionMessageQueue = new Queue<NetIncomingMessage>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.scene.Group.MessageHandler["setup:begin"] = this.HandleSetupStartMessage;
            this.scene.Group.MessageHandler["setup:end"] = this.HandleSetupEndMessage;
            this.scene.Group.MessageHandler["action"] = this.EnqueueActionMessage;
            this.scene.Group.MessageHandler["update"] = this.EnqueueUpdateMessage;
            this.scene.Group.MessageHandler["create"] = this.HandleCreateMessage;
        }

        #region NetMessage Handlers
        private void HandleSetupStartMessage(NetIncomingMessage obj)
        {
            this.logger.LogInformation("Starting NetworkScene Setup...");
        }

        private void HandleSetupEndMessage(NetIncomingMessage obj)
        {
            this.logger.LogInformation("Ending NetworkScene Setup...");

            this.scene.Group.MessageHandler["action"] = this.scene.HandleActionMessage;
            this.scene.Group.MessageHandler["update"] = this.HandleUpdateMessage;

            // Flush the collected queue while the client was setting up
            while (_updateMessageQueue.Count > 0)
                this.HandleUpdateMessage(_updateMessageQueue.Dequeue());
            while (_actionMessageQueue.Count > 0)
                this.scene.HandleActionMessage(_actionMessageQueue.Dequeue());

            // Empty the message queue
            _updateMessageQueue.Clear();
            _actionMessageQueue.Clear();

            this.logger.LogInformation("Ended NetworkScene Setup.");
        }

        private void HandleCreateMessage(NetIncomingMessage obj)
        {
            var ne = _entities.Create<NetworkEntity>(obj.ReadString(), obj.ReadGuid());
        }
        private void HandleUpdateMessage(NetIncomingMessage obj)
        {
            var id = obj.ReadGuid();
            var ne = _networkEntities.GetById(id);

            if(ne == null)
            {
                this.logger.LogError($"Unable to run update message. Unknown NetworkEntity({id})");
                return;
            }

            ne.Read(obj);

            // Mark the entity as clean now
            ne.Dirty = false;
        }

        /// <summary>
        /// Special message queues used to hold group methods
        /// recieved before setup is complete.
        /// </summary>
        /// <param name="obj"></param>
        private void EnqueueUpdateMessage(NetIncomingMessage obj)
        {
            _updateMessageQueue.Enqueue(obj);
        }
        private void EnqueueActionMessage(NetIncomingMessage obj)
        {
            _actionMessageQueue.Enqueue(obj);
        }
        #endregion
    }
}
