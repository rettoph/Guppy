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

        public ClientNetworkSceneDriver(NetworkScene scene, NetworkEntityCollection networkEntities, EntityCollection entities, IServiceProvider provider, ILogger logger) : base(scene, provider, logger)
        {
            _networkEntities = networkEntities;
            _entities = entities;
        }

        protected override void Boot()
        {
            base.Boot();

            this.scene.Group.MessageHandler.Add("setup:begin", this.HandleSetupStartMessage);
            this.scene.Group.MessageHandler.Add("setup:end", this.HandleSetupEndMessage);
        }

        #region NetMessage Handlers
        private void HandleSetupStartMessage(NetIncomingMessage obj)
        {
            _updateMessageQueue = new Queue<NetIncomingMessage>();
            _actionMessageQueue = new Queue<NetIncomingMessage>();

            this.scene.Group.MessageHandler["action"] = this.EnqueueActionMessage;
            this.scene.Group.MessageHandler["update"] = this.EnqueueUpdateMessage;
            this.scene.Group.MessageHandler["create"] = this.HandleCreateMessage;

        }
        private void HandleSetupEndMessage(NetIncomingMessage obj)
        {
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
        }

        private void HandleCreateMessage(NetIncomingMessage obj)
        {
            var ne = _entities.Create<NetworkEntity>(obj.ReadString(), obj.ReadGuid());
        }
        private void HandleUpdateMessage(NetIncomingMessage obj)
        {
            var ne = _networkEntities.GetById(obj.ReadGuid());
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
            this.logger.LogInformation("New update message recieved!");
            _updateMessageQueue.Enqueue(obj);
        }
        private void EnqueueActionMessage(NetIncomingMessage obj)
        {
            _actionMessageQueue.Enqueue(obj);
        }
        #endregion
    }
}
